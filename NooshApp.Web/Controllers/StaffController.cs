using Microsoft.AspNetCore.Mvc;
using NooshApp.Web.Helpers;
using NooshApp.Web.Services;

namespace NooshApp.Web.Controllers
{
    // Deliberately not linked anywhere in navigation — reachable only by
    // a bookmarked/typed URL, gated by the staff PIN itself.
    public class StaffController : Controller
    {
        private readonly IStaffApiClient _staffApiClient;
        public StaffController(IStaffApiClient staffApiClient) { _staffApiClient = staffApiClient; }

        [HttpGet]
        public IActionResult Login() => View();

        [HttpPost]
        public async Task<IActionResult> Login(string pin)
        {
            // Confirm the PIN actually works by making one harmless real call
            // rather than duplicating the PIN value in Web's own config.
            var probe = await _staffApiClient.ScanAsync(pin, "pin-check-only", 0);

            // Any non-401-style rejection reaching here means the PIN itself
            // was accepted by the Api (the request just failed for an
            // unrelated reason — a fake token). If the PIN were wrong, the
            // Api's StaffPinFilter would have rejected before that logic ran.
            if (probe.Message == "Invalid staff PIN.")
            {
                ModelState.AddModelError(string.Empty, "Incorrect PIN.");
                return View();
            }

            HttpContext.Session.SetStaffPin(pin);
            return RedirectToAction("Scan");
        }

        public IActionResult Scan()
        {
            if (!HttpContext.Session.IsStaffAuthenticated())
                return RedirectToAction("Login");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SubmitScan(string token, decimal amountPaid)
        {
            var pin = HttpContext.Session.GetStaffPin();
            if (string.IsNullOrEmpty(pin)) return Unauthorized();

            var result = await _staffApiClient.ScanAsync(pin, token, amountPaid);
            return Json(result);
        }

        public async Task<IActionResult> Redeem()
        {
            if (!HttpContext.Session.IsStaffAuthenticated())
                return RedirectToAction("Login");

            ViewBag.Rewards = await _staffApiClient.GetActiveRewardsAsync();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SubmitRedeem(string email, int rewardRuleId)
        {
            var pin = HttpContext.Session.GetStaffPin();
            if (string.IsNullOrEmpty(pin)) return Unauthorized();

            var result = await _staffApiClient.RedeemAsync(pin, email, rewardRuleId);
            return Json(result);
        }

        public IActionResult Logout()
        {
            HttpContext.Session.ClearStaffPin();
            return RedirectToAction("Login");
        }
    }
}