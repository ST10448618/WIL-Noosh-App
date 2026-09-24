using NooshApp.Web.Dtos;

namespace NooshApp.Web.ViewModels
{
    public class MenuViewModel
    {
        public List<MenuItemDto> Items { get; set; } = new();
        public List<string> Categories { get; set; } = new();
    }
}