using System.ComponentModel.DataAnnotations;

namespace NooshApp.Api.Models
{

    /// Current state of the catering request.
    public enum CateringStatus
    {
        New,
        Contacted,
        Confirmed,
        Declined
    }

    public class CateringRequest
    {
        [Key]
        public int Id { get; set; }
    
        [Required, MaxLength(100)]
        public string FullName { get; set; } = string.Empty;

        [Required, MaxLength(15)]
        public string PhoneNumber { get; set; } = string.Empty;

        [Required, MaxLength(150)]
        public string Email { get; set; } = string.Empty;

        [Required]
        public DateTime EventDate { get; set; }

        [Required, Range(1, 1000)]
        public int GuestCount { get; set; }

        [Required, MaxLength(200)]
        public string EventLocation { get; set; } = string.Empty;

        [MaxLength(1000)]
        public string? AdditionalNotes { get; set; }

        /// Current state of the catering request.

        public CateringStatus Status { get; set; } = CateringStatus.New;
        public DateTime SubmittedAt { get; set; } = DateTime.UtcNow;
    }
}