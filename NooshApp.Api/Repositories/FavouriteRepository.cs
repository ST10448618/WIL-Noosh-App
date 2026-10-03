using Microsoft.EntityFrameworkCore;
using NooshApp.Api.Data;
using NooshApp.Api.Models;
using NooshApp.Api.Repositories.Interfaces;

namespace NooshApp.Api.Repositories
{
    public class FavouriteRepository : IFavouriteRepository
    {
        private readonly ApplicationDbContext _context;
        public FavouriteRepository(ApplicationDbContext context) { _context = context; }

        public async Task<List<Favourite>> GetByCustomerIdAsync(int customerId) =>
            await _context.Favourites.Include(f => f.MenuItem)
                .Where(f => f.CustomerId == customerId).ToListAsync();

        public async Task<Favourite?> FindAsync(int customerId, int menuItemId) =>
            await _context.Favourites.FirstOrDefaultAsync(f => f.CustomerId == customerId && f.MenuItemId == menuItemId);

        public async Task AddAsync(Favourite favourite)
        {
            await _context.Favourites.AddAsync(favourite);
            await _context.SaveChangesAsync();
        }

        public async Task RemoveAsync(Favourite favourite)
        {
            _context.Favourites.Remove(favourite);
            await _context.SaveChangesAsync();
        }
    }
}