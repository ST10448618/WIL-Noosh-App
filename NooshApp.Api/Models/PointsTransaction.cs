using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NooshApp.Api.Models
{
    public enum PointsSource { StaffScan, SelfServiceReceipt, Redemption, AdminAdjustment }

    public class PointsTransaction
    {
        [Key] public int Id { get; set; }
        [Required] public int CustomerId { get; set; }
        [ForeignKey(nameof(CustomerId))] public Customer? Customer { get; set; }
        [Required] public int Amount { get; set; } // + earned, - redeemed
        [MaxLength(200)] public string Description { get; set; } = string.Empty;
        public PointsSource Source { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}