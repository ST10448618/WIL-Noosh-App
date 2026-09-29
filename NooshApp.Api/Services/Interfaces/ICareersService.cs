using Microsoft.AspNetCore.Http;
using NooshApp.Api.Dtos;

namespace NooshApp.Api.Services.Interfaces
{
    public interface ICareersService
    {
        Task<JobApplicationDto> SubmitApplicationAsync(
            string fullName, string phoneNumber, string email, string desiredPosition,
            string? coverLetter, IFormFile cvFile, List<IFormFile>? supportingDocuments);
    }
}