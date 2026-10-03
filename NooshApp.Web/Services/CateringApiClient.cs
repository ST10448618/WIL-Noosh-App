using System.Net.Http.Json;
using NooshApp.Web.Dtos;

namespace NooshApp.Web.Services
{
    public class CateringApiClient : ICateringApiClient
    {
        private readonly HttpClient _httpClient;
        public CateringApiClient(HttpClient httpClient) { _httpClient = httpClient; }
     
     public async Task<CateringRequestDto> SubmitAsync(CateringRequestCreateDto request)
        {
            var response = await _httpClient.PostAsJsonAsync("api/catering", request);
            response.EnsureSuccessStatusCode();
            return (await response.Content.ReadFromJsonAsync<CateringRequestDto>())!;
        }

        public async Task<CateringRequestDto?> GetByIdAsync(int id) =>
            await _httpClient.GetFromJsonAsync<CateringRequestDto>($"api/catering/{id}");
    }
}