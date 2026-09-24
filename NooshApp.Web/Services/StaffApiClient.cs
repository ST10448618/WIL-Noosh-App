using System.Net.Http.Json;
using NooshApp.Web.Dtos;

namespace NooshApp.Web.Services
{
    public class StaffApiClient : IStaffApiClient
    {
        private readonly HttpClient _httpClient;
        public StaffApiClient(HttpClient httpClient) { _httpClient = httpClient; }

        private HttpRequestMessage BuildRequest(HttpMethod method, string path, string staffPin, object? body = null)
        {
            var request = new HttpRequestMessage(method, path);
            request.Headers.Add("X-Staff-Pin", staffPin);
            if (body != null) request.Content = JsonContent.Create(body);
            return request;
        }

        public async Task<ScanResultDto> ScanAsync(string staffPin, string token, decimal amountPaid)
        {
            var request = BuildRequest(HttpMethod.Post, "api/rewards/scan", staffPin, new { token, amountPaid });
            var response = await _httpClient.SendAsync(request);
            var result = await response.Content.ReadFromJsonAsync<ScanResultDto>();
            return result ?? new ScanResultDto { Success = false, Message = "Unexpected response from server." };
        }

        public async Task<ScanResultDto> RedeemAsync(string staffPin, string email, int rewardRuleId)
        {
            var request = BuildRequest(HttpMethod.Post, "api/rewards/redeem", staffPin, new { email, rewardRuleId });
            var response = await _httpClient.SendAsync(request);
            var result = await response.Content.ReadFromJsonAsync<ScanResultDto>();
            return result ?? new ScanResultDto { Success = false, Message = "Unexpected response from server." };
        }

        public async Task<List<RewardRuleDto>> GetActiveRewardsAsync() =>
            await _httpClient.GetFromJsonAsync<List<RewardRuleDto>>("api/reward-rules") ?? new();
    }
}