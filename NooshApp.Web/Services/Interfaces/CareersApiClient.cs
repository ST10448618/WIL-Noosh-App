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
        
         using var cvStream = model.CvFile.OpenReadStream();
            using var cvContent = new StreamContent(cvStream);
            cvContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(model.CvFile.ContentType);
            content.Add(cvContent, "cvFile", model.CvFile.FileName);

            var docStreams = new List<Stream>(); // kept alive until the request completes

            if (model.SupportingDocuments != null)
            {
                foreach (var doc in model.SupportingDocuments.Where(d => d.Length > 0))
                {
                    var docStream = doc.OpenReadStream();
                    docStreams.Add(docStream);
                    var docContent = new StreamContent(docStream);
                    docContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(doc.ContentType);
                    content.Add(docContent, "supportingDocuments", doc.FileName);
                }
            }
        
        }
    }
}