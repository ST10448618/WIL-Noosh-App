using NooshApp.Web.Dtos;

namespace NooshApp.Web.Services
{
    public interface IMenuApiClient
    {
        Task<List<MenuItemDto>> GetFeaturedAsync();
        Task<List<MenuItemDto>> GetAllAsync();
    }
}