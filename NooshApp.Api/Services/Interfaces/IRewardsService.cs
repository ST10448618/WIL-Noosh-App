using NooshApp.Api.Models;
using NooshApp.Api.Dtos;

namespace NooshApp.Api.Services.Interfaces
{
    public interface IRewardsService
    {
        Task<(string token, string qrImageBase64)> GenerateQrAsync(string email, string? fullName);
        Task<PointsResultDto> RedeemScanTokenAsync(string token, decimal amountPaid);
        Task<PointsResultDto> SubmitReceiptAsync(string email, string? fullName, string receiptReference, decimal amountPaid, DateOnly purchaseDate);
        Task<PointsResultDto> RedeemRewardAsync(string email, int rewardRuleId);
        Task<(int balance, List<PointsTransaction> history)> GetAccountAsync(string email);
    }
}