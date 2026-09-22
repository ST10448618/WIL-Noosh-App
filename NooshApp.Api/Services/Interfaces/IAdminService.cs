using NooshApp.Api.Dtos;
using NooshApp.Api.Models;
using Microsoft.AspNetCore.Http;

namespace NooshApp.Api.Services.Interfaces
{
    public interface IAdminService
    {
        Task<List<RewardRule>> GetAllRewardRulesAsync();
        Task<RewardRule> CreateRewardRuleAsync(string name, int pointsRequired, string description);
        Task<RewardRule?> UpdateRewardRuleAsync(int id, string name, int pointsRequired, string description, bool isActive, int displayOrder);
        Task DeleteRewardRuleAsync(int id);
        Task<AppSettings> GetSettingsAsync();
        Task UpdatePointsPerRandAsync(decimal pointsPerRand);
        Task DeleteMenuItemAsync(int id);
        Task<string?> UploadMenuItemImageAsync(int id, IFormFile image);
    }
}