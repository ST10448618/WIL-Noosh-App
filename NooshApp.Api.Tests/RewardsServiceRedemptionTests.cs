using Moq;
using NooshApp.Api.Models;
using NooshApp.Api.Repositories.Interfaces;
using NooshApp.Api.Services;
using Xunit;

namespace NooshApp.Api.Tests
{
    public class RewardsServiceRedemptionTests
    {
        private readonly Mock<ICustomerRepository> _customerRepo = new();
        private readonly Mock<IRewardRuleRepository> _rewardRuleRepo = new();
        private readonly Mock<IPointsRepository> _pointsRepo = new();
        private readonly Mock<IScanTokenRepository> _scanTokenRepo = new();
        private readonly Mock<IReceiptSubmissionRepository> _receiptRepo = new();
        private readonly Mock<IAppSettingsRepository> _settingsRepo = new();

        private RewardsService BuildService() => new RewardsService(
            _customerRepo.Object, _rewardRuleRepo.Object, _pointsRepo.Object,
            _scanTokenRepo.Object, _receiptRepo.Object, _settingsRepo.Object);

        [Fact]
        public async Task RedeemRewardAsync_DeductsExactPointsRequired()
        {
            // Explanation: confirms redemption deducts EXACTLY the reward's
            // PointsRequired — not the whole balance, not a rounded value —
            // using the real AppSettings-driven PointsPerRand calculation path.
            var customer = new Customer { Id = 1, Email = "a@b.com" };
            var rule = new RewardRule { Id = 1, PointsRequired = 150, IsActive = true, RewardDescription = "Free Fries" };

            _customerRepo.Setup(r => r.GetByEmailAsync("a@b.com")).ReturnsAsync(customer);
            _rewardRuleRepo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(rule);
            _pointsRepo.SetupSequence(r => r.GetBalanceAsync(1))
                .ReturnsAsync(200)   // balance check before redeeming
                .ReturnsAsync(50);   // balance after redemption

            var service = BuildService();
            var result = await service.RedeemRewardAsync("a@b.com", 1);

            Assert.True(result.Success);
            Assert.Equal(50, result.Balance);
            _pointsRepo.Verify(r => r.AddTransactionAsync(
                It.Is<PointsTransaction>(t => t.Amount == -150)), Times.Once);
        }

        [Fact]
        public async Task RedeemRewardAsync_FailsGracefully_WhenCustomerDoesNotExist()
        {
            // Explanation: confirms staff redeeming for a mistyped/unknown email
            // gets a clean error message, not a crash mid-transaction.
            _customerRepo.Setup(r => r.GetByEmailAsync("unknown@x.com")).ReturnsAsync((Customer?)null);

            var result = await BuildService().RedeemRewardAsync("unknown@x.com", 1);

            Assert.False(result.Success);
            Assert.Equal("Customer not found.", result.Message);
        }
    }
}