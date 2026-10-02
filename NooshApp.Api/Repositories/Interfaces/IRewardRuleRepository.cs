using NooshApp.Api.Models;
namespace NooshApp.Api.Repositories.Interfaces
{
    public interface IRewardRuleRepository
    {
        Task<List<RewardRule>> GetActiveAsync();
        Task<List<RewardRule>> GetAllAsync();
        Task<RewardRule?> GetByIdAsync(int id);
        Task<RewardRule> CreateAsync(RewardRule rule);
        Task UpdateAsync(RewardRule rule);
        Task DeleteAsync(int id);
    }
}