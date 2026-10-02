using System.ComponentModel.DataAnnotations;

namespace NooshApp.Api.Models
{
    public class RewardRule
    {
        [Key] public int Id { get; set; }
        [Required, MaxLength(150)] public string Name { get; set; } = string.Empty;
        [Required] public int PointsRequired { get; set; }
        [Required, MaxLength(200)] public string RewardDescription { get; set; } = string.Empty;
        public bool IsActive { get; set; } = true;
        public bool IsDeleted { get; set; } = false;
        public int DisplayOrder { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}