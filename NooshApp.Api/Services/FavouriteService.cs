using NooshApp.Api.Dtos;
using NooshApp.Api.Models;
using NooshApp.Api.Repositories.Interfaces;
using NooshApp.Api.Services.Interfaces;

namespace NooshApp.Api.Services
{
    public class FavouriteService : IFavouriteService
    {
        private readonly IFavouriteRepository _favouriteRepository;
        private readonly ICustomerRepository _customerRepository;
        private readonly IMenuItemRepository _menuItemRepository;

        public FavouriteService(IFavouriteRepository favouriteRepository, ICustomerRepository customerRepository, IMenuItemRepository menuItemRepository)
        {
            _favouriteRepository = favouriteRepository;
            _customerRepository = customerRepository;
            _menuItemRepository = menuItemRepository;
        }

        public async Task<List<FavouriteMenuItemDto>> GetFavouritesAsync(string email)
        {
            var customer = await _customerRepository.GetByEmailAsync(email);
            if (customer == null) return new();

            var favourites = await _favouriteRepository.GetByCustomerIdAsync(customer.Id);
            return favourites.Where(f => f.MenuItem != null).Select(f => new FavouriteMenuItemDto
            {
                MenuItemId = f.MenuItemId,
                Name = f.MenuItem!.Name,
                Price = f.MenuItem.Price,
                ImageUrl = f.MenuItem.ImageUrl,
                Category = f.MenuItem.Category
            }).ToList();
        }

        public async Task<bool> ToggleFavouriteAsync(string email, int menuItemId)
        {
            var customer = await _customerRepository.GetByEmailAsync(email);
            customer ??= await _customerRepository.CreateAsync(email, null);

            var existing = await _favouriteRepository.FindAsync(customer.Id, menuItemId);
            if (existing != null)
            {
                await _favouriteRepository.RemoveAsync(existing);
                return false;
            }

            await _favouriteRepository.AddAsync(new Favourite { CustomerId = customer.Id, MenuItemId = menuItemId });
            return true;
        }
    }
}