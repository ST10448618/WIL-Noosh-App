using System.Net.Http.Json;
using NooshApp.Web.Dtos;

namespace NooshApp.Web.Services
{
    public class AdminApiClient : IAdminApiClient
    {
        private readonly HttpClient _httpClient;
        public AdminApiClient(HttpClient httpClient) { _httpClient = httpClient; }

        private HttpRequestMessage BuildRequest(HttpMethod method, string path, string adminKey, object? body = null)
        {
            var request = new HttpRequestMessage(method, path);
            request.Headers.Add("X-Admin-Key", adminKey);
            if (body != null) request.Content = JsonContent.Create(body);
            return request;
        }

        public async Task<List<AdminRewardRuleDto>> GetAllRewardRulesAsync(string adminKey)
        {
            var request = BuildRequest(HttpMethod.Get, "api/admin/reward-rules", adminKey);
            var response = await _httpClient.SendAsync(request);
            if (!response.IsSuccessStatusCode) return new();
            return await response.Content.ReadFromJsonAsync<List<AdminRewardRuleDto>>() ?? new();
        }

        public async Task<bool> CreateRewardRuleAsync(string adminKey, string name, int pointsRequired, string description)
        {
            var request = BuildRequest(HttpMethod.Post, "api/admin/reward-rules", adminKey,
                new { name, pointsRequired, rewardDescription = description });
            var response = await _httpClient.SendAsync(request);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> UpdateRewardRuleAsync(string adminKey, int id, string name, int pointsRequired, string description, bool isActive, int displayOrder)
        {
            var request = BuildRequest(HttpMethod.Put, $"api/admin/reward-rules/{id}", adminKey,
                new { name, pointsRequired, rewardDescription = description, isActive, displayOrder });
            var response = await _httpClient.SendAsync(request);
            return response.IsSuccessStatusCode;
        }

        public async Task<AppSettingsDto> GetSettingsAsync(string adminKey)
        {
            var request = BuildRequest(HttpMethod.Get, "api/admin/settings", adminKey);
            var response = await _httpClient.SendAsync(request);
            if (!response.IsSuccessStatusCode) return new AppSettingsDto();
            return await response.Content.ReadFromJsonAsync<AppSettingsDto>() ?? new AppSettingsDto();
        }

        public async Task<bool> UpdateSettingsAsync(string adminKey, decimal pointsPerRand)
        {
            var request = BuildRequest(HttpMethod.Put, "api/admin/settings", adminKey, new { pointsPerRand });
            var response = await _httpClient.SendAsync(request);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteRewardRuleAsync(string adminKey, int id)
        {
            var request = BuildRequest(HttpMethod.Delete, $"api/admin/reward-rules/{id}", adminKey);
            var response = await _httpClient.SendAsync(request);
            return response.IsSuccessStatusCode;
        }

        public async Task<List<MenuItemAdminDto>> GetAllMenuItemsAsync(string adminKey)
        {
            var request = BuildRequest(HttpMethod.Get, "api/admin/menu-items", adminKey);
            var response = await _httpClient.SendAsync(request);
            if (!response.IsSuccessStatusCode) return new();
            return await response.Content.ReadFromJsonAsync<List<MenuItemAdminDto>>() ?? new();
        }

        public async Task<bool> CreateMenuItemAsync(string adminKey, object payload)
        {
            var request = BuildRequest(HttpMethod.Post, "api/admin/menu-items", adminKey, payload);
            var response = await _httpClient.SendAsync(request);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> UpdateMenuItemAsync(string adminKey, int id, object payload)
        {
            var request = BuildRequest(HttpMethod.Put, $"api/admin/menu-items/{id}", adminKey, payload);
            var response = await _httpClient.SendAsync(request);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteMenuItemAsync(string adminKey, int id)
        {
            var request = BuildRequest(HttpMethod.Delete, $"api/admin/menu-items/{id}", adminKey);
            var response = await _httpClient.SendAsync(request);
            return response.IsSuccessStatusCode;
        }

        public async Task<string?> UploadMenuItemImageAsync(string adminKey, int id, IFormFile image)
        {
            using var content = new MultipartFormDataContent();
            using var stream = image.OpenReadStream();
            using var fileContent = new StreamContent(stream);
            fileContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(image.ContentType);
            content.Add(fileContent, "image", image.FileName);

            var request = new HttpRequestMessage(HttpMethod.Post, $"api/admin/menu-items/{id}/image") { Content = content };
            request.Headers.Add("X-Admin-Key", adminKey);

            var response = await _httpClient.SendAsync(request);
            if (!response.IsSuccessStatusCode) return null;

            var result = await response.Content.ReadFromJsonAsync<Dictionary<string, string>>();
            return result != null && result.TryGetValue("imageUrl", out var url) ? url : null;
        }
    }
}