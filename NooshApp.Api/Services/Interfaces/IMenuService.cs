using NooshApp.Api.Models;
using NooshApp.Api.Dtos;

namespace NooshApp.Api.Services.Interfaces
{
    
    // Business logic layer for menu-related operations.
    // Controllers talk to this, never directly to the Repository.
    
    public interface IMenuService
    {
        Task<List<MenuItemDto>> GetFeaturedMealsAsync();
        Task<List<MenuItemDto>> GetFullMenuAsync();
        Task<List<MenuItemDto>> GetMenuByCategoryAsync(string category);
    }
}