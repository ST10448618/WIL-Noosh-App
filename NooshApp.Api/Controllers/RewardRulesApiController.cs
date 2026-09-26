using Microsoft.AspNetCore.Mvc;
using NooshApp.Api.Repositories.Interfaces;

namespace NooshApp.Api.Controllers
{
    [ApiController]
    [Route("api/reward-rules")]
    public class RewardRulesApiController : ControllerBase
    {
        private readonly IRewardRuleRepository _rewardRuleRepository;
        public RewardRulesApiController(IRewardRuleRepository rewardRuleRepository) { _rewardRuleRepository = rewardRuleRepository; }

        [HttpGet]
        public async Task<IActionResult> GetActive() => Ok(await _rewardRuleRepository.GetActiveAsync());
    }
}