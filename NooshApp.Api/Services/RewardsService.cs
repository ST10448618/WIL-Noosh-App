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

    }
}