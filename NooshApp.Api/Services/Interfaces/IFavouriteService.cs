using NooshApp.Api.Dtos;

namespace NooshApp.Api.Services.Interfaces
{
    public interface IFavouriteService
    {
        Task<List<FavouriteMenuItemDto>> GetFavouritesAsync(string email);
        Task<bool> ToggleFavouriteAsync(string email, int menuItemId); // returns true if now favourited
    }
}