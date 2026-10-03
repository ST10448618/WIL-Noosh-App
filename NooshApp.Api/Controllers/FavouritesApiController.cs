using Microsoft.AspNetCore.Mvc;
using NooshApp.Api.Auth;
using NooshApp.Api.Services.Interfaces;

namespace NooshApp.Api.Controllers
{
    [ApiController]
    [Route("api/favourites")]
    [ServiceFilter(typeof(FirebaseAuthFilter))]
    public class FavouritesApiController : ControllerBase
    {
        private readonly IFavouriteService _favouriteService;
        public FavouritesApiController(IFavouriteService favouriteService) { _favouriteService = favouriteService; }

        private string GetVerifiedEmail() =>
            HttpContext.Items["VerifiedEmail"] as string ?? throw new InvalidOperationException("Not verified.");

        [HttpGet]
        public async Task<IActionResult> GetFavourites() =>
            Ok(await _favouriteService.GetFavouritesAsync(GetVerifiedEmail()));

        [HttpPost("{menuItemId}")]
        public async Task<IActionResult> Toggle(int menuItemId)
        {
            var isFavourited = await _favouriteService.ToggleFavouriteAsync(GetVerifiedEmail(), menuItemId);
            return Ok(new { isFavourited });
        }
    }
}