using System.ComponentModel.DataAnnotations;

namespace NooshApp.Web.ViewModels
{
    public class CateringRequestViewModel
    {
        [Required(ErrorMessage = "Please enter your full name.")]
        [Display(Name = "Full Name")]
        public string FullName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Please enter your phone number.")]
        [Phone(ErrorMessage = "Please enter a valid phone number.")]
        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; } = string.Empty;

        [Required(ErrorMessage = "Please enter your email address.")]
        [EmailAddress(ErrorMessage = "Please enter a valid email address.")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Please select an event date.")]
        [DataType(DataType.Date)]
        [Display(Name = "Event Date")]
        public DateTime EventDate { get; set; } = DateTime.Today.AddDays(7);

        [Required(ErrorMessage = "Please enter the number of guests.")]
        [Range(1, 1000, ErrorMessage = "Guest count must be between 1 and 1000.")]
        [Display(Name = "Number of Guests")]
        public int GuestCount { get; set; } = 20;

        [Required(ErrorMessage = "Please enter the event location.")]
        [Display(Name = "Event Location")]
        public string EventLocation { get; set; } = string.Empty;

        [MaxLength(1000, ErrorMessage = "Notes must be under 1000 characters.")]
        [Display(Name = "Additional Notes (Optional)")]
        public string? AdditionalNotes { get; set; }
    }
}