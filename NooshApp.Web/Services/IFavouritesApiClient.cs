using NooshApp.Web.Dtos;

namespace NooshApp.Web.Services
{
    public interface IFavouritesApiClient
    {
        Task<List<FavouriteMenuItemDto>> GetFavouritesAsync(string idToken);
        Task<bool> ToggleFavouriteAsync(string idToken, int menuItemId);
    }
}