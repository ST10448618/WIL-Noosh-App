using Microsoft.AspNetCore.Mvc;
using NooshApp.Web.Helpers;
using NooshApp.Web.Services;

namespace NooshApp.Web.Controllers
{
    // Not linked in navigation — reachable only by direct URL, gated by
    // the admin key itself (same pattern as Staff).
    public class AdminController : Controller
    {
        private readonly IAdminApiClient _adminApiClient;
        public AdminController(IAdminApiClient adminApiClient) { _adminApiClient = adminApiClient; }

        [HttpGet]
        public IActionResult Login() => View();

        [HttpPost]
        public async Task<IActionResult> Login(string key)
        {
            var rules = await _adminApiClient.GetAllRewardRulesAsync(key);
            // GetAllRewardRulesAsync returns an empty list on a 401 — a genuinely
            // empty (but authorized) list would be indistinguishable from a wrong
            // key on the very first use before any rules exist. Acceptable here
            // since we always seed default reward rules, so an empty result
            // reliably means "key was rejected," not "no rules configured."
            if (!rules.Any())
            {
                ModelState.AddModelError(string.Empty, "Incorrect admin key.");
                return View();
            }

            HttpContext.Session.SetAdminKey(key);
            return RedirectToAction("Rewards");
        }

        public async Task<IActionResult> Rewards()
        {
            if (!HttpContext.Session.IsAdminAuthenticated()) return RedirectToAction("Login");

            var adminKey = HttpContext.Session.GetAdminKey()!;
            ViewBag.Rules = await _adminApiClient.GetAllRewardRulesAsync(adminKey);
            ViewBag.Settings = await _adminApiClient.GetSettingsAsync(adminKey);
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateRule(string name, int pointsRequired, string description)
        {
            var adminKey = HttpContext.Session.GetAdminKey();
            if (string.IsNullOrEmpty(adminKey)) return Unauthorized();

            await _adminApiClient.CreateRewardRuleAsync(adminKey, name, pointsRequired, description);
            return RedirectToAction("Rewards");
        }

        [HttpPost]
        public async Task<IActionResult> UpdateRule(int id, string name, int pointsRequired, string description, bool isActive, int displayOrder)
        {
            var adminKey = HttpContext.Session.GetAdminKey();
            if (string.IsNullOrEmpty(adminKey)) return Unauthorized();

            await _adminApiClient.UpdateRewardRuleAsync(adminKey, id, name, pointsRequired, description, isActive, displayOrder);
            return RedirectToAction("Rewards");
        }

        [HttpPost]
        public async Task<IActionResult> UpdateSettings(decimal pointsPerRand)
        {
            var adminKey = HttpContext.Session.GetAdminKey();
            if (string.IsNullOrEmpty(adminKey)) return Unauthorized();

            await _adminApiClient.UpdateSettingsAsync(adminKey, pointsPerRand);
            return RedirectToAction("Rewards");
        }

        public IActionResult Logout()
        {
            HttpContext.Session.ClearAdminKey();
            return RedirectToAction("Login");
        }

        [HttpPost]
        public async Task<IActionResult> DeleteRule(int id)
        {
            var adminKey = HttpContext.Session.GetAdminKey();
            if (string.IsNullOrEmpty(adminKey)) return Unauthorized();

            await _adminApiClient.DeleteRewardRuleAsync(adminKey, id);
            return RedirectToAction("Rewards");
        }

        public async Task<IActionResult> MenuItems()
        {
            if (!HttpContext.Session.IsAdminAuthenticated()) return RedirectToAction("Login");
            var adminKey = HttpContext.Session.GetAdminKey()!;
            ViewBag.MenuItems = await _adminApiClient.GetAllMenuItemsAsync(adminKey);
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateMenuItem(string name, string? description, decimal price, string category,
            bool isPopular, bool isVegetarian, int spiceLevel, bool containsEggs, bool containsWheat, bool containsDairy, bool containsSesame)
        {
            var adminKey = HttpContext.Session.GetAdminKey();
            if (string.IsNullOrEmpty(adminKey)) return Unauthorized();

            await _adminApiClient.CreateMenuItemAsync(adminKey, new
            {
                name, description, price, category, isPopular, isVegetarian, spiceLevel,
                containsEggs, containsWheat, containsDairy, containsSesame
            });
            return RedirectToAction("MenuItems");
        }

        [HttpPost]
        public async Task<IActionResult> UpdateMenuItem(int id, string name, string? description, decimal price, string category,
            bool isPopular, bool isVegetarian, int spiceLevel, bool containsEggs, bool containsWheat, bool containsDairy, bool containsSesame, bool isAvailable)
        {
            var adminKey = HttpContext.Session.GetAdminKey();
            if (string.IsNullOrEmpty(adminKey)) return Unauthorized();

            await _adminApiClient.UpdateMenuItemAsync(adminKey, id, new
            {
                name, description, price, category, isPopular, isVegetarian, spiceLevel,
                containsEggs, containsWheat, containsDairy, containsSesame, isAvailable
            });
            return RedirectToAction("MenuItems");
        }

        [HttpPost]
        public async Task<IActionResult> DeleteMenuItem(int id)
        {
            var adminKey = HttpContext.Session.GetAdminKey();
            if (string.IsNullOrEmpty(adminKey)) return Unauthorized();

            await _adminApiClient.DeleteMenuItemAsync(adminKey, id);
            return RedirectToAction("MenuItems");
        }

        [HttpPost]
        public async Task<IActionResult> UploadMenuItemImage(int id, IFormFile image)
        {
            var adminKey = HttpContext.Session.GetAdminKey();
            if (string.IsNullOrEmpty(adminKey)) return Unauthorized();

            if (image == null || image.Length == 0)
            {
                TempData["UploadError"] = "No file was selected.";
                return RedirectToAction("MenuItems");
            }

            var resultUrl = await _adminApiClient.UploadMenuItemImageAsync(adminKey, id, image);

            if (resultUrl == null)
            {
                TempData["UploadError"] = $"Upload failed for item {id} — check file type (.jpg/.jpeg/.png only) and size (max 5MB).";
            }

            return RedirectToAction("MenuItems");
        }
    }
}