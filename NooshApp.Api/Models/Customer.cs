using System.ComponentModel.DataAnnotations;

namespace NooshApp.Api.Models
{
    public class Customer
    {
        [Key] public int Id { get; set; }
        [Required, MaxLength(150)] public string Email { get; set; } = string.Empty;
        [MaxLength(100)] public string? FullName { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}