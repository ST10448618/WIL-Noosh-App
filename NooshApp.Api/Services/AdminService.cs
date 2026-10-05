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
        private readonly IMenuItemRepository _menuItemRepository;
        private readonly IWebHostEnvironment _environment;

        private static readonly string[] AllowedImageExtensions = { ".jpg", ".jpeg", ".png" };
        private const long MaxImageSizeBytes = 5 * 1024 * 1024;

        public AdminService(
            IRewardRuleRepository rewardRuleRepository,
            IAppSettingsRepository appSettingsRepository,
            IMenuItemRepository menuItemRepository,
            IWebHostEnvironment environment)
        {
            _rewardRuleRepository = rewardRuleRepository;
            _appSettingsRepository = appSettingsRepository;
            _menuItemRepository = menuItemRepository;
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

        // ---- new: menu item management ----

        public async Task<List<MenuItem>> GetAllMenuItemsAsync() => await _menuItemRepository.GetAllForAdminAsync();

        public async Task<MenuItem> CreateMenuItemAsync(CreateMenuItemRequestDto request)
        {
            var item = new MenuItem
            {
                Name = request.Name,
                Description = request.Description,
                Price = request.Price,
                Category = request.Category,
                IsPopular = request.IsPopular,
                IsVegetarian = request.IsVegetarian,
                SpiceLevel = (SpiceLevel)request.SpiceLevel,
                ContainsEggs = request.ContainsEggs,
                ContainsWheat = request.ContainsWheat,
                ContainsDairy = request.ContainsDairy,
                ContainsSesame = request.ContainsSesame,
                IsAvailable = true
            };
            return await _menuItemRepository.CreateAsync(item);
        }

        public async Task<MenuItem?> UpdateMenuItemAsync(int id, UpdateMenuItemRequestDto request)
        {
            var item = await _menuItemRepository.GetByIdAsync(id);
            if (item == null) return null;

            item.Name = request.Name;
            item.Description = request.Description;
            item.Price = request.Price;
            item.Category = request.Category;
            item.IsPopular = request.IsPopular;
            item.IsVegetarian = request.IsVegetarian;
            item.SpiceLevel = (SpiceLevel)request.SpiceLevel;
            item.ContainsEggs = request.ContainsEggs;
            item.ContainsWheat = request.ContainsWheat;
            item.ContainsDairy = request.ContainsDairy;
            item.ContainsSesame = request.ContainsSesame;
            item.IsAvailable = request.IsAvailable;

            await _menuItemRepository.UpdateAsync(item);
            return item;
        }

        public async Task DeleteMenuItemAsync(int id) => await _menuItemRepository.DeleteAsync(id);

        public async Task<string?> UploadMenuItemImageAsync(int id, IFormFile image)
        {
            var item = await _menuItemRepository.GetByIdAsync(id);
            if (item == null) return null;

            var extension = Path.GetExtension(image.FileName).ToLowerInvariant();
            if (!AllowedImageExtensions.Contains(extension) || image.Length > MaxImageSizeBytes)
                return null;

            var uploadsFolder = Path.Combine(_environment.WebRootPath ?? "wwwroot", "images", "menu");
            Directory.CreateDirectory(uploadsFolder);
            var fileName = $"{Guid.NewGuid()}{extension}";
            var fullPath = Path.Combine(uploadsFolder, fileName);

            using (var stream = new FileStream(fullPath, FileMode.Create))
                await image.CopyToAsync(stream);

            item.ImageUrl = $"/images/menu/{fileName}";
            await _menuItemRepository.UpdateAsync(item);

            return item.ImageUrl;
        }

        public async Task UpdateMenuItemImageUrlAsync(int id, string absoluteImageUrl)
        {
            var item = await _menuItemRepository.GetByIdAsync(id);
            if (item == null) return;
            item.ImageUrl = absoluteImageUrl;
            await _menuItemRepository.UpdateAsync(item);
        }
    }
}