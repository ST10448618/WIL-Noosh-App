using NooshApp.Api.Models;
using NooshApp.Api.Repositories.Interfaces;
using NooshApp.Api.Dtos;
using NooshApp.Api.Services.Interfaces;
using QRCoder;

namespace NooshApp.Api.Services
{
    public class RewardsService : IRewardsService
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IRewardRuleRepository _rewardRuleRepository;
        private readonly IPointsRepository _pointsRepository;
        private readonly IScanTokenRepository _scanTokenRepository;
        private readonly IReceiptSubmissionRepository _receiptSubmissionRepository;
        private readonly IAppSettingsRepository _appSettingsRepository;
        private static readonly TimeSpan QrTokenLifetime = TimeSpan.FromMinutes(2);

        public RewardsService(ICustomerRepository customerRepository, IRewardRuleRepository rewardRuleRepository,
            IPointsRepository pointsRepository, IScanTokenRepository scanTokenRepository,
            IReceiptSubmissionRepository receiptSubmissionRepository, IAppSettingsRepository appSettingsRepository)
        {
            _customerRepository = customerRepository;
            _rewardRuleRepository = rewardRuleRepository;
            _pointsRepository = pointsRepository;
            _scanTokenRepository = scanTokenRepository;
            _receiptSubmissionRepository = receiptSubmissionRepository;
            _appSettingsRepository = appSettingsRepository;
        }

        private async Task<Customer> GetOrCreateCustomerAsync(string email, string? fullName)
        {
            var customer = await _customerRepository.GetByEmailAsync(email);
            return customer ?? await _customerRepository.CreateAsync(email, fullName);
        }

        private async Task<int> CalculatePointsAsync(decimal amountPaid)
        {
            var settings = await _appSettingsRepository.GetAsync();
            return (int)Math.Floor(amountPaid * settings.PointsPerRand);
        }

        public async Task<(string token, string qrImageBase64)> GenerateQrAsync(string email, string? fullName)
        {
            var customer = await GetOrCreateCustomerAsync(email, fullName);
            var token = await _scanTokenRepository.CreateAsync(customer.Id, QrTokenLifetime);

            using var generator = new QRCodeGenerator();
            using var qrData = generator.CreateQrCode(token.Token, QRCodeGenerator.ECCLevel.Q);
            var pngQr = new PngByteQRCode(qrData);
            return (token.Token, Convert.ToBase64String(pngQr.GetGraphic(10)));
        }

        public async Task<PointsResultDto> RedeemScanTokenAsync(string token, decimal amountPaid)
        {
            var scanToken = await _scanTokenRepository.GetByTokenAsync(token);
            if (scanToken == null) return Fail("This QR code is not recognized.");
            if (scanToken.IsUsed) return Fail("This QR code has already been used.");
            if (DateTime.UtcNow > scanToken.ExpiresAt) return Fail("This QR code has expired.");
            if (amountPaid <= 0) return Fail("Enter the sale amount before scanning.");

            await _scanTokenRepository.MarkUsedAsync(scanToken);
            var points = await CalculatePointsAsync(amountPaid);

            await _pointsRepository.AddTransactionAsync(new PointsTransaction
            {
                CustomerId = scanToken.CustomerId, Amount = points,
                Description = $"In-store purchase (R{amountPaid:0.00})", Source = PointsSource.StaffScan
            });

            var balance = await _pointsRepository.GetBalanceAsync(scanToken.CustomerId);
            return new PointsResultDto { Success = true, Message = $"+{points} points added!", Balance = balance };
        }

        public async Task<PointsResultDto> SubmitReceiptAsync(string email, string? fullName, string receiptReference, decimal amountPaid, DateOnly purchaseDate)
        {
            if (await _receiptSubmissionRepository.ExistsAsync(receiptReference, amountPaid, purchaseDate))
                return Fail("This receipt has already been used to claim points.");

            var customer = await GetOrCreateCustomerAsync(email, fullName);
            await _receiptSubmissionRepository.AddAsync(new ReceiptSubmission
            {
                CustomerId = customer.Id, ReceiptReference = receiptReference,
                AmountPaid = amountPaid, PurchaseDate = purchaseDate
            });

            var points = await CalculatePointsAsync(amountPaid);
            await _pointsRepository.AddTransactionAsync(new PointsTransaction
            {
                CustomerId = customer.Id, Amount = points,
                Description = $"Receipt #{receiptReference} (R{amountPaid:0.00})", Source = PointsSource.SelfServiceReceipt
            });

            var balance = await _pointsRepository.GetBalanceAsync(customer.Id);
            return new PointsResultDto { Success = true, Message = $"+{points} points added!", Balance = balance };
        }

        public async Task<PointsResultDto> RedeemRewardAsync(string email, int rewardRuleId)
        {
            var customer = await _customerRepository.GetByEmailAsync(email);
            if (customer == null) return Fail("Customer not found.");

            var rule = await _rewardRuleRepository.GetByIdAsync(rewardRuleId);
            if (rule == null || !rule.IsActive) return Fail("This reward is not currently available.");

            var balance = await _pointsRepository.GetBalanceAsync(customer.Id);
            if (balance < rule.PointsRequired) return Fail($"Not enough points. Has {balance}, needs {rule.PointsRequired}.");

            await _pointsRepository.AddTransactionAsync(new PointsTransaction
            {
                CustomerId = customer.Id, Amount = -rule.PointsRequired,
                Description = $"Redeemed: {rule.RewardDescription}", Source = PointsSource.Redemption
            });

            var newBalance = await _pointsRepository.GetBalanceAsync(customer.Id);
            return new PointsResultDto { Success = true, Message = $"Redeemed: {rule.RewardDescription}", Balance = newBalance };
        }

        public async Task<(int balance, List<PointsTransaction> history)> GetAccountAsync(string email)
        {
            var customer = await _customerRepository.GetByEmailAsync(email);
            if (customer == null) return (0, new List<PointsTransaction>());
            var balance = await _pointsRepository.GetBalanceAsync(customer.Id);
            var history = await _pointsRepository.GetHistoryAsync(customer.Id);
            return (balance, history);
        }

        private PointsResultDto Fail(string message) => new PointsResultDto { Success = false, Message = message };
    }
}