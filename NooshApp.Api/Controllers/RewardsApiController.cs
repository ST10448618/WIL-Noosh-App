using Microsoft.AspNetCore.Mvc;
using NooshApp.Api.Auth;
using NooshApp.Api.Dtos;
using NooshApp.Api.Services.Interfaces;

namespace NooshApp.Api.Controllers
{
    [ApiController]
    [Route("api/rewards")]
    public class RewardsApiController : ControllerBase
    {
        private readonly IRewardsService _rewardsService;
        public RewardsApiController(IRewardsService rewardsService) { _rewardsService = rewardsService; }

        private string GetVerifiedEmail() =>
            HttpContext.Items["VerifiedEmail"] as string ?? throw new InvalidOperationException("Not verified.");

        [HttpPost("generate-qr")]
        [ServiceFilter(typeof(FirebaseAuthFilter))]
        public async Task<IActionResult> GenerateQr([FromBody] GenerateQrRequestDto request)
        {
            var email = GetVerifiedEmail();
            var (token, qrImageBase64) = await _rewardsService.GenerateQrAsync(email, request.FullName);
            return Ok(new { token, qrImageDataUrl = $"data:image/png;base64,{qrImageBase64}" });
        }

        [HttpGet("balance")]
        [ServiceFilter(typeof(FirebaseAuthFilter))]
        public async Task<IActionResult> GetBalance()
        {
            var email = GetVerifiedEmail();
            var (balance, history) = await _rewardsService.GetAccountAsync(email);
            return Ok(new { balance, history });
        }

        [HttpPost("scan")]
        [ServiceFilter(typeof(StaffPinFilter))]
        public async Task<IActionResult> Scan([FromBody] ScanRequestDto request)
        {
            var result = await _rewardsService.RedeemScanTokenAsync(request.Token, request.AmountPaid);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpPost("submit-receipt")]
        [ServiceFilter(typeof(FirebaseAuthFilter))]
        public async Task<IActionResult> SubmitReceipt([FromBody] SubmitReceiptRequestDto request)
        {
            var email = GetVerifiedEmail();
            var result = await _rewardsService.SubmitReceiptAsync(email, request.FullName, request.ReceiptReference, request.AmountPaid, request.PurchaseDate);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpPost("redeem")]
        [ServiceFilter(typeof(StaffPinFilter))]
        public async Task<IActionResult> Redeem([FromBody] StaffRedeemRequestDto request)
        {
            var result = await _rewardsService.RedeemRewardAsync(request.Email, request.RewardRuleId);
            return result.Success ? Ok(result) : BadRequest(result);
        }
    }
}