using NooshApp.Api.Models;

namespace NooshApp.Api.Repositories.Interfaces
{
    public interface IFavouriteRepository
    {
        Task<List<Favourite>> GetByCustomerIdAsync(int customerId);
        Task<Favourite?> FindAsync(int customerId, int menuItemId);
        Task AddAsync(Favourite favourite);
        Task RemoveAsync(Favourite favourite);
    }
}