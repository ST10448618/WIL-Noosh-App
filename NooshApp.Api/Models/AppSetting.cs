using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NooshApp.Api.Models
{
    public class AppSettings
    {
        [Key] public int Id { get; set; }
        [Required, Column(TypeName = "decimal(6,2)")] public decimal PointsPerRand { get; set; } = 0.1m; // R150 → 15pts
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}