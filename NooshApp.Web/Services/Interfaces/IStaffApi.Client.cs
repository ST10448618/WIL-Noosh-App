using NooshApp.Web.Dtos;

namespace NooshApp.Web.Services
{
    public interface IStaffApiClient
    {
        Task<ScanResultDto> ScanAsync(string email, string token, decimal amountPaid);
        Task<ScanResultDto> RedeemAsync(string staffPin, string email, int rewardRuleId);
        Task<List<RewardRuleDto>> GetActiveRewardsAsync();
    }
}