using NooshApp.Web.Dtos;

namespace NooshApp.Web.ViewModels
{
    public class HomeViewModel
    {
        public List<MenuItemDto> FeaturedMeals { get; set; } = new();
        public List<StoreLocation> StoreLocations { get; set; } = new();
    }
}