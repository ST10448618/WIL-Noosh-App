using NooshApp.Api.Models;

namespace NooshApp.Api.Repositories.Interfaces
{
    //Data operations are defined for Menu Items
    //API controllers will talk to the interface instead of the the EF Directly
    public interface IMenuItemRepository
    {
        Task<List<MenuItem>> GetAllAsync();
        Task<List<MenuItem>> GetByCategoryAsync(string category);
        Task<List<MenuItem>> GetFeaturedAsync(int count);
        Task<List<MenuItem>> GetAllForAdminAsync();
        Task<MenuItem?> GetByIdAsync(int id);
        Task<MenuItem> CreateAsync(MenuItem item);
        Task UpdateAsync(MenuItem item);
        Task DeleteAsync(int id);
    }
}