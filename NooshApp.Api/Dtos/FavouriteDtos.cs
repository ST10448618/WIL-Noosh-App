namespace NooshApp.Api.Dtos
{
    public class FavouriteMenuItemDto
    {
        public int MenuItemId { get; set; }
        public string Name { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public string? ImageUrl { get; set; }
        public string Category { get; set; } = string.Empty;
    }
}