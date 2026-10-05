namespace NooshApp.Api.Dtos
{

    //The DTO is a simple container object used to pass data 
    //between different layers of the Noosh App.
    public class MenuItemDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public string Category { get; set; } = string.Empty;
        public string? ImageUrl { get; set; }
        public bool IsPopular { get; set; }
        public bool IsVegetarian { get; set; }
        public int SpiceLevel { get; set; }
        public bool ContainsEggs { get; set; }
        public bool ContainsWheat { get; set; }
        public bool ContainsDairy { get; set; }
        public bool ContainsSesame { get; set; }

    }
}