using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace NooshApp.Web.ViewModels
{
    public class CareerApplicationViewModel
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

        [Required(ErrorMessage = "Please tell us which position you're applying for.")]
        [Display(Name = "Desired Position")]
        public string DesiredPosition { get; set; } = string.Empty;

        [MaxLength(2000, ErrorMessage = "Cover letter must be under 2000 characters.")]
        [Display(Name = "Cover Letter (Optional)")]
        public string? CoverLetter { get; set; }

        [Display(Name = "Supporting Documents (Optional)")]
        public List<IFormFile>? SupportingDocuments { get; set; }

        [Required(ErrorMessage = "Please upload your CV.")]
        [Display(Name = "Upload CV (PDF or Word)")]
        public IFormFile CvFile { get; set; } = null!;
    }
}