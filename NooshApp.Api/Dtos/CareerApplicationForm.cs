using Microsoft.AspNetCore.Http;

namespace NooshApp.Api.Dtos
{
    
    // Binds the entire multipart/form-data submission as one model.
    // Swashbuckle cannot reliably generate a schema when loose [FromForm]
    // scalar parameters are mixed with IFormFile parameters on the same
    // action — binding to a single class resolves that.

    public class CareerApplicationForm
    {
        public string FullName { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string DesiredPosition { get; set; } = string.Empty;
        public string? CoverLetter { get; set; }
        public IFormFile CvFile { get; set; } = null!;
        public List<IFormFile>? SupportingDocuments { get; set; }
    }
}