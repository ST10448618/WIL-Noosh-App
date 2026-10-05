using Microsoft.AspNetCore.Mvc;
using NooshApp.Web.Helpers;
using NooshApp.Web.Services;

namespace NooshApp.Web.Controllers
{
    public class FavouritesController : Controller
    {
        private readonly IFavouritesApiClient _favouritesApiClient;
        public FavouritesController(IFavouritesApiClient favouritesApiClient) { _favouritesApiClient = favouritesApiClient; }

        public async Task<IActionResult> Index()
        {
            if (!HttpContext.IsLoggedIn()) return RedirectToAction("Login", "Account");
            var idToken = HttpContext.GetIdToken()!;
            var favourites = await _favouritesApiClient.GetFavouritesAsync(idToken);
            return View(favourites);
        }

        [HttpPost]
        public async Task<IActionResult> Toggle(int menuItemId)
        {
            if (!HttpContext.IsLoggedIn()) return Unauthorized();
            var idToken = HttpContext.GetIdToken()!;
            var isFavourited = await _favouritesApiClient.ToggleFavouriteAsync(idToken, menuItemId);
            return Json(new { isFavourited });
        }
    }
}