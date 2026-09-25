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
    }
}