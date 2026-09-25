using NooshApp.Web.Dtos;

namespace NooshApp.Web.Services
{
    public interface IAdminApiClient
    {
        Task<List<AdminRewardRuleDto>> GetAllRewardRulesAsync(string adminKey);
        Task<bool> CreateRewardRuleAsync(string adminKey, string name, int pointsRequired, string description);
        Task<bool> UpdateRewardRuleAsync(string adminKey, int id, string name, int pointsRequired, string description, bool isActive, int displayOrder);
        Task<AppSettingsDto> GetSettingsAsync(string adminKey);
        Task<bool> UpdateSettingsAsync(string adminKey, decimal pointsPerRand);
        Task<bool> DeleteRewardRuleAsync(string adminKey, int id);
        Task<List<MenuItemAdminDto>> GetAllMenuItemsAsync(string adminKey);
        Task<bool> CreateMenuItemAsync(string adminKey, object payload);
        Task<bool> UpdateMenuItemAsync(string adminKey, int id, object payload);
        Task<bool> DeleteMenuItemAsync(string adminKey, int id);
        Task<string?> UploadMenuItemImageAsync(string adminKey, int id, IFormFile image);
    }
}