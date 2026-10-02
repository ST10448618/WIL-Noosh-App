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

    }
}