using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NooshApp.Api.Models
{
    public class ReceiptSubmission
    {
        [Key] public int Id { get; set; }
        [Required] public int CustomerId { get; set; }
        [ForeignKey(nameof(CustomerId))] public Customer? Customer { get; set; }
        [Required, MaxLength(50)] public string ReceiptReference { get; set; } = string.Empty;
        [Required, Column(TypeName = "decimal(8,2)")] public decimal AmountPaid { get; set; }
        [Required] public DateOnly PurchaseDate { get; set; }
        public DateTime SubmittedAt { get; set; } = DateTime.UtcNow;
    }
}