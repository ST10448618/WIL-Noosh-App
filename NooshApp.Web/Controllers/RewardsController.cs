using Microsoft.AspNetCore.Mvc;
using NooshApp.Web.Helpers;
using NooshApp.Web.Services;
using NooshApp.Web.ViewModels;

namespace NooshApp.Web.Controllers
{
    public class RewardsController : Controller
    {
        private readonly IRewardsApiClient _rewardsApiClient;
        private readonly ILogger<RewardsController> _logger;
        public RewardsController(IRewardsApiClient rewardsApiClient, ILogger<RewardsController> logger) { _rewardsApiClient = rewardsApiClient; _logger = logger; }

        public async Task<IActionResult> Index()
        {
            if (!HttpContext.Session.IsLoggedIn())
                return RedirectToAction("Login", "Account");

            var idToken = HttpContext.Session.GetIdToken()!;
            var balance = await _rewardsApiClient.GetBalanceAsync(idToken);
            var rewards = await _rewardsApiClient.GetActiveRewardsAsync();

            return View(new RewardsDashboardViewModel
            {
                Balance = balance.Balance,
                History = balance.History,
                AvailableRewards = rewards
            });
        }

        [HttpPost]
        public async Task<IActionResult> GenerateQr()
        {
            if (!HttpContext.Session.IsLoggedIn()) return Unauthorized();
            var idToken = HttpContext.Session.GetIdToken()!;

            try
            {
                var result = await _rewardsApiClient.GenerateQrAsync(idToken, null);
                return result == null ? BadRequest(new { message = "The rewards service didn't respond correctly." }) : Json(result);
            }
            catch (Exception ex)
            {
                // Logs the REAL reason (timeout, DNS failure, 401, etc.) instead of
                // silently returning null and leaving you guessing.
                _logger.LogError(ex, "GenerateQr failed calling the Rewards API.");
                return StatusCode(502, new { message = "Could not reach the rewards service. Please try again." });
            }
        }
    }
}