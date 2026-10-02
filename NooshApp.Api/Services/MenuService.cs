using NooshApp.Api.Dtos;
using NooshApp.Api.Models;
using NooshApp.Api.Repositories.Interfaces;
using NooshApp.Api.Services.Interfaces;

namespace NooshApp.Api.Services
{
    public class MenuService : IMenuService
    {
        private readonly IMenuItemRepository _repository;
        private const int FeaturedMealCount = 4;
        public MenuService(IMenuItemRepository repository) { _repository = repository; }
        //// Asynchronously retrieves a limited set of featured meals and maps them to DTOs
        public async Task<List<MenuItemDto>> GetFeaturedMealsAsync() =>
            (await _repository.GetFeaturedAsync(FeaturedMealCount)).Select(ToDto).ToList();

        public async Task<List<MenuItemDto>> GetFullMenuAsync() =>
            (await _repository.GetAllAsync()).Select(ToDto).ToList();

        public async Task<List<MenuItemDto>> GetMenuByCategoryAsync(string category) =>
            (await _repository.GetByCategoryAsync(category)).Select(ToDto).ToList();

        //Helper mapper method: converts a database Entity (MenuItem) into a 
        //Data Transfer Object (MenuItemDto)
        private static MenuItemDto ToDto(MenuItem item) => new MenuItemDto
        {
            Id = item.Id,
            Name = item.Name,
            Description = item.Description,
            Price = item.Price,
            Category = item.Category,
            ImageUrl = item.ImageUrl,
            IsPopular = item.IsPopular,
            IsVegetarian = item.IsVegetarian,
            SpiceLevel = (int)item.SpiceLevel, //Casts the enum or value to an integer for the DTO
            ContainsEggs = item.ContainsEggs,
            ContainsWheat = item.ContainsWheat,
            ContainsDairy = item.ContainsDairy,
            ContainsSesame = item.ContainsSesame
        };
    }
}