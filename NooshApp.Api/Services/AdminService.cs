using Microsoft.AspNetCore.Http;
using NooshApp.Api.Dtos;
using NooshApp.Api.Models;
using NooshApp.Api.Repositories.Interfaces;
using NooshApp.Api.Services.Interfaces;

namespace NooshApp.Api.Services
{
    public class AdminService : IAdminService
    {
        private readonly IRewardRuleRepository _rewardRuleRepository;
        private readonly IAppSettingsRepository _appSettingsRepository;
        private readonly IWebHostEnvironment _environment;

        private static readonly string[] AllowedImageExtensions = { ".jpg", ".jpeg", ".png" };
        private const long MaxImageSizeBytes = 5 * 1024 * 1024;

        public AdminService(
            IRewardRuleRepository rewardRuleRepository,
            IAppSettingsRepository appSettingsRepository,
            IWebHostEnvironment environment)
        {
            _rewardRuleRepository = rewardRuleRepository;
            _appSettingsRepository = appSettingsRepository;
            _environment = environment;
        }

        // ---- existing reward rule / settings methods stay exactly as they are ----
        public async Task<List<RewardRule>> GetAllRewardRulesAsync() => await _rewardRuleRepository.GetAllAsync();

        public async Task<RewardRule> CreateRewardRuleAsync(string name, int pointsRequired, string description)
        {
            var rule = new RewardRule
            {
                Name = name, PointsRequired = pointsRequired, RewardDescription = description,
                IsActive = true, DisplayOrder = (await _rewardRuleRepository.GetAllAsync()).Count + 1
            };
            return await _rewardRuleRepository.CreateAsync(rule);
        }

        public async Task<RewardRule?> UpdateRewardRuleAsync(int id, string name, int pointsRequired, string description, bool isActive, int displayOrder)
        {
            var rule = await _rewardRuleRepository.GetByIdAsync(id);
            if (rule == null) return null;
            rule.Name = name; rule.PointsRequired = pointsRequired; rule.RewardDescription = description;
            rule.IsActive = isActive; rule.DisplayOrder = displayOrder;
            await _rewardRuleRepository.UpdateAsync(rule);
            return rule;
        }

        public async Task DeleteRewardRuleAsync(int id) => await _rewardRuleRepository.DeleteAsync(id);

        public async Task<AppSettings> GetSettingsAsync() => await _appSettingsRepository.GetAsync();
        public async Task UpdatePointsPerRandAsync(decimal pointsPerRand) => await _appSettingsRepository.UpdateAsync(pointsPerRand);

    }
}