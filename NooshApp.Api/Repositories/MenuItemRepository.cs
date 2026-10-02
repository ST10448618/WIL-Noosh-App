using Microsoft.EntityFrameworkCore;
using NooshApp.Api.Data;
using NooshApp.Api.Models;
using NooshApp.Api.Repositories.Interfaces;

namespace NooshApp.Api.Repositories
{
    public class MenuItemRepository : IMenuItemRepository
    {
        private readonly ApplicationDbContext _context;
        public MenuItemRepository(ApplicationDbContext context) { _context = context; }

        public async Task<List<MenuItem>> GetAllAsync() =>
            await _context.MenuItems.Where(m => m.IsAvailable && !m.IsDeleted)
                .OrderBy(m => m.Category).ThenBy(m => m.Name).ToListAsync();

        public async Task<List<MenuItem>> GetByCategoryAsync(string category) =>
            await _context.MenuItems.Where(m => m.IsAvailable && !m.IsDeleted && m.Category == category)
                .OrderBy(m => m.Name).ToListAsync();

        public async Task<List<MenuItem>> GetFeaturedAsync(int count)
        {
            var featuredItems = await _context.MenuItems
                .Where(m => m.IsAvailable && !m.IsDeleted && m.IsPopular).ToListAsync();
            return featuredItems.OrderBy(m => Guid.NewGuid()).Take(count).ToList();
        }

        public async Task<List<MenuItem>> GetAllForAdminAsync() =>
            await _context.MenuItems.Where(m => !m.IsDeleted)
                .OrderBy(m => m.Category).ThenBy(m => m.Name).ToListAsync();

        public async Task<MenuItem?> GetByIdAsync(int id) => await _context.MenuItems.FindAsync(id);

        public async Task<MenuItem> CreateAsync(MenuItem item)
        {
            await _context.MenuItems.AddAsync(item);
            await _context.SaveChangesAsync();
            return item;
        }

        public async Task UpdateAsync(MenuItem item)
        {
            _context.MenuItems.Update(item);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var item = await _context.MenuItems.FindAsync(id);
            if (item != null)
            {
                item.IsDeleted = true;
                _context.MenuItems.Update(item);
                await _context.SaveChangesAsync();
            }
        }
    }
}