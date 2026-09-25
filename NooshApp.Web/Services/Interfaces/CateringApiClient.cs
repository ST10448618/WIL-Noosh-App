using System.Net.Http.Json;
using NooshApp.Web.Dtos;

namespace NooshApp.Web.Services
{
    public class CateringApiClient : ICateringApiClient
    {
        private readonly HttpClient _httpClient;
        public CateringApiClient(HttpClient httpClient) { _httpClient = httpClient; }
    }
}