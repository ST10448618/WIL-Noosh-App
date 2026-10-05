using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NooshApp.Api.Models
{
    public class Favourite
    {
        [Key] public int Id { get; set; }
        [Required] public int CustomerId { get; set; }
        [ForeignKey(nameof(CustomerId))] public Customer? Customer { get; set; }
        [Required] public int MenuItemId { get; set; }
        [ForeignKey(nameof(MenuItemId))] public MenuItem? MenuItem { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}