using System.Net.Http.Headers;
using System.Net.Http.Json;
using NooshApp.Web.Dtos;

namespace NooshApp.Web.Services
{
    public class RewardsApiClient : IRewardsApiClient
    {
        private readonly HttpClient _httpClient;
        public RewardsApiClient(HttpClient httpClient) { _httpClient = httpClient; }

        public async Task<List<RewardRuleDto>> GetActiveRewardsAsync() =>
            await _httpClient.GetFromJsonAsync<List<RewardRuleDto>>("api/reward-rules") ?? new();

        public async Task<BalanceResponseDto> GetBalanceAsync(string idToken)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, "api/rewards/balance");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", idToken);
            var response = await _httpClient.SendAsync(request);
            if (!response.IsSuccessStatusCode) return new BalanceResponseDto();
            return await response.Content.ReadFromJsonAsync<BalanceResponseDto>() ?? new BalanceResponseDto();
        }

        public async Task<GenerateQrResponseDto?> GenerateQrAsync(string idToken, string? fullName)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, "api/rewards/generate-qr")
            {
                Content = JsonContent.Create(new { fullName })
            };
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", idToken);
            var response = await _httpClient.SendAsync(request);
            if (!response.IsSuccessStatusCode) return null;
            return await response.Content.ReadFromJsonAsync<GenerateQrResponseDto>();
        }
    }
}