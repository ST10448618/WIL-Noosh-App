using Microsoft.AspNetCore.Mvc;
using NooshApp.Api.Dtos;
using NooshApp.Api.Services.Interfaces;

namespace NooshApp.Api.Controllers
{
    // Marks this class as an API controller and sets the base route path to "/api/careers"
    [ApiController]
    [Route("api/careers")]
    public class CareersApiController : ControllerBase
    {
        // Reference to the careers service for handling job application business logic
        private readonly ICareersService _careersService;

        // Constructor injection to receive the careers service implementation
        public CareersApiController(ICareersService careersService) { _careersService = careersService; }

        // HTTP POST endpoint to handle job application submissions at POST /api/careers/apply
        [HttpPost("apply")]
        [RequestSizeLimit(25 * 1024 * 1024)] // Enforces an absolute maximum HTTP request body size limit of 25MB for file uploads
        public async Task<IActionResult> Apply([FromForm] CareerApplicationForm form)
        {
            // Validate that the uploaded CV file has an allowed file extension (.pdf or .docx)
            var allowedExtensions = new[] { ".pdf", ".docx" };
            var extension = Path.GetExtension(form.CvFile.FileName).ToLowerInvariant();

            if (!allowedExtensions.Contains(extension))
                return BadRequest(new { message = "Please upload a PDF or Word (.docx) file for your CV." });

            // Validate that the CV file size does not exceed the 5MB limit
            const int maxFileSizeBytes = 5 * 1024 * 1024;
            if (form.CvFile.Length > maxFileSizeBytes)
                return BadRequest(new { message = "CV file is too large. Maximum size is 5MB." });

            // Pass form field data and files to the careers service layer for processing and storage
            var result = await _careersService.SubmitApplicationAsync(
                form.FullName, form.PhoneNumber, form.Email, form.DesiredPosition,
                form.CoverLetter, form.CvFile, form.SupportingDocuments);

            // Return an HTTP 200 OK response with the created application details
            return Ok(result);
        }
    }
}