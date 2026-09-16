using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NooshApp.Api.Models
{
        public enum SpiceLevel
    {
        None = 0,
        Mild = 1,
        Medium = 2,
        Hot = 3,
        ExtraHot = 4
    }
    /// <summary>
    /// Represents a single item on the restaurant menu (e.g. "Chicken Shawarma").
    /// </summary>
    public class MenuItem
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        [MaxLength(500)]
        public string? Description { get; set; }

        [Required]
        [Column(TypeName = "decimal(6,2)")]
        public decimal Price { get; set; }

        [Required]
        [MaxLength(50)]
        public string Category { get; set; } = string.Empty; // e.g. "Shawarmas", "Wraps", "Drinks"

        [MaxLength(300)]
        public string? ImageUrl { get; set; }

        public bool IsPopular { get; set; } = false;
        public bool IsVegetarian { get; set; } = false;
        public SpiceLevel SpiceLevel { get; set; } = SpiceLevel.None;
        public bool IsAvailable { get; set; } = true; // lets admin hide items without deleting them

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public bool ContainsEggs { get; set; } = false;
        public bool ContainsWheat { get; set; } = false;
        public bool ContainsDairy { get; set; } = false;
        public bool ContainsSesame { get; set; } = false;
    }
}