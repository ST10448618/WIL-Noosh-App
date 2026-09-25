using System.Net.Http.Json;
using NooshApp.Web.ViewModels;

namespace NooshApp.Web.Services
{
    public class CareersApiClient : ICareersApiClient
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<CareersApiClient> _logger;

        public CareersApiClient(HttpClient httpClient, ILogger<CareersApiClient> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

          public async Task<CareersApplyResult> SubmitApplicationAsync(CareerApplicationViewModel model)
        {
            using var content = new MultipartFormDataContent();
            content.Add(new StringContent(model.FullName), "fullName");
            content.Add(new StringContent(model.PhoneNumber), "phoneNumber");
            content.Add(new StringContent(model.Email), "email");
            content.Add(new StringContent(model.DesiredPosition), "desiredPosition");
            content.Add(new StringContent(model.CoverLetter ?? string.Empty), "coverLetter");
        }
    }
}