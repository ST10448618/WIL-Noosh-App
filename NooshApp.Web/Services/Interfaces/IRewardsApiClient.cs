using NooshApp.Web.Dtos;

namespace NooshApp.Web.Services
{
    public interface IRewardsApiClient
    {
        Task<List<RewardRuleDto>> GetActiveRewardsAsync();
        Task<BalanceResponseDto> GetBalanceAsync(string idToken);
        Task<GenerateQrResponseDto?> GenerateQrAsync(string idToken, string? fullName);
    }
}