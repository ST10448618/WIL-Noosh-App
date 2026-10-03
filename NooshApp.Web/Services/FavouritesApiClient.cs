using System.Net.Http.Headers;
using System.Net.Http.Json;
using NooshApp.Web.Dtos;

namespace NooshApp.Web.Services
{
    public class FavouritesApiClient : IFavouritesApiClient
    {
        private readonly HttpClient _httpClient;
        public FavouritesApiClient(HttpClient httpClient) { _httpClient = httpClient; }

        private HttpRequestMessage Auth(HttpMethod method, string path, string idToken)
        {
            var request = new HttpRequestMessage(method, path);
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", idToken);
            return request;
        }

        public async Task<List<FavouriteMenuItemDto>> GetFavouritesAsync(string idToken)
        {
            var response = await _httpClient.SendAsync(Auth(HttpMethod.Get, "api/favourites", idToken));
            if (!response.IsSuccessStatusCode) return new();
            return await response.Content.ReadFromJsonAsync<List<FavouriteMenuItemDto>>() ?? new();
        }

        public async Task<bool> ToggleFavouriteAsync(string idToken, int menuItemId)
        {
            var response = await _httpClient.SendAsync(Auth(HttpMethod.Post, $"api/favourites/{menuItemId}", idToken));
            if (!response.IsSuccessStatusCode) return false;
            var result = await response.Content.ReadFromJsonAsync<Dictionary<string, bool>>();
            return result != null && result.TryGetValue("isFavourited", out var val) && val;
        }
    }
}