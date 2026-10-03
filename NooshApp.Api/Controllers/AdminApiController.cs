using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NooshApp.Api.Auth;
using NooshApp.Api.Dtos;
using NooshApp.Api.Services.Interfaces;

namespace NooshApp.Api.Controllers
{
    [ApiController]
    [Route("api/admin")]
    [ServiceFilter(typeof(AdminKeyFilter))]
    public class AdminApiController : ControllerBase
    {
        private readonly IAdminService _adminService;
        public AdminApiController(IAdminService adminService) { _adminService = adminService; }

        [HttpGet("reward-rules")]
        public async Task<IActionResult> GetAllRewardRules() => Ok(await _adminService.GetAllRewardRulesAsync());

        [HttpPost("reward-rules")]
        public async Task<IActionResult> CreateRewardRule([FromBody] CreateRewardRuleRequestDto request)
        {
            var rule = await _adminService.CreateRewardRuleAsync(request.Name, request.PointsRequired, request.RewardDescription);
            return Ok(rule);
        }

        [HttpPut("reward-rules/{id}")]
        public async Task<IActionResult> UpdateRewardRule(int id, [FromBody] UpdateRewardRuleRequestDto request)
        {
            var rule = await _adminService.UpdateRewardRuleAsync(id, request.Name, request.PointsRequired, request.RewardDescription, request.IsActive, request.DisplayOrder);
            return rule == null ? NotFound() : Ok(rule);
        }

        [HttpDelete("reward-rules/{id}")]
        public async Task<IActionResult> DeleteRewardRule(int id)
        {
            await _adminService.DeleteRewardRuleAsync(id);
            return Ok(new { message = "Reward deleted." });
        }

        [HttpGet("settings")]
        public async Task<IActionResult> GetSettings() => Ok(await _adminService.GetSettingsAsync());

        [HttpPut("settings")]
        public async Task<IActionResult> UpdateSettings([FromBody] UpdateSettingsRequestDto request)
        {
            await _adminService.UpdatePointsPerRandAsync(request.PointsPerRand);
            return Ok(new { message = "Settings updated." });
        }

        [HttpGet("menu-items")]
        public async Task<IActionResult> GetAllMenuItems()
        {
            var items = await _adminService.GetAllMenuItemsAsync();
            return Ok(items.Select(ToAdminDto));
        }

        [HttpPost("menu-items")]
        public async Task<IActionResult> CreateMenuItem([FromBody] CreateMenuItemRequestDto request)
        {
            var item = await _adminService.CreateMenuItemAsync(request);
            return Ok(ToAdminDto(item));
        }

        [HttpPut("menu-items/{id}")]
        public async Task<IActionResult> UpdateMenuItem(int id, [FromBody] UpdateMenuItemRequestDto request)
        {
            var item = await _adminService.UpdateMenuItemAsync(id, request);
            return item == null ? NotFound() : Ok(ToAdminDto(item));
        }

        [HttpDelete("menu-items/{id}")]
        public async Task<IActionResult> DeleteMenuItem(int id)
        {
            await _adminService.DeleteMenuItemAsync(id);
            return Ok(new { message = "Menu item deleted." });
        }

        [HttpPost("menu-items/{id}/image")]
        public async Task<IActionResult> UploadMenuItemImage(int id, IFormFile image)
        {
            var relativeUrl = await _adminService.UploadMenuItemImageAsync(id, image);
            if (relativeUrl == null)
                return BadRequest(new { message = "Invalid image (must be JPG/PNG, max 5MB) or item not found." });

            // The image physically lives on THIS Api server, but is rendered by the
            // separate NooshApp.Web frontend — a relative path resolves against the
            // wrong origin there. Store the full absolute URL instead.
            var absoluteUrl = $"{Request.Scheme}://{Request.Host}{relativeUrl}";
            await _adminService.UpdateMenuItemImageUrlAsync(id, absoluteUrl);

            return Ok(new { imageUrl = absoluteUrl });
        }

        private static MenuItemAdminDto ToAdminDto(NooshApp.Api.Models.MenuItem item) => new()
        {
            Id = item.Id, Name = item.Name, Description = item.Description, Price = item.Price,
            Category = item.Category, ImageUrl = item.ImageUrl, IsPopular = item.IsPopular,
            IsVegetarian = item.IsVegetarian, SpiceLevel = (int)item.SpiceLevel,
            ContainsEggs = item.ContainsEggs, ContainsWheat = item.ContainsWheat,
            ContainsDairy = item.ContainsDairy, ContainsSesame = item.ContainsSesame,
            IsAvailable = item.IsAvailable
        };
    }
}