using System.Net.Http.Json;
using NooshApp.Web.Dtos;

namespace NooshApp.Web.Services
{
    /// <summary>
    /// Talks to NooshApp.Api over HTTP. Controllers depend on this interface
    /// exactly the way they used to depend on IMenuService directly —
    /// the Controller code barely changes, only what's behind the interface does.
    /// </summary>
    public class MenuApiClient : IMenuApiClient
    {
        private readonly HttpClient _httpClient;
        public MenuApiClient(HttpClient httpClient) { _httpClient = httpClient; }

        public async Task<List<MenuItemDto>> GetFeaturedAsync() =>
            await _httpClient.GetFromJsonAsync<List<MenuItemDto>>("api/menu/featured") ?? new();

        public async Task<List<MenuItemDto>> GetAllAsync() =>
            await _httpClient.GetFromJsonAsync<List<MenuItemDto>>("api/menu") ?? new();
    }
}