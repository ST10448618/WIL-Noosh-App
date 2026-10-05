using NooshApp.Web.Dtos;

namespace NooshApp.Web.ViewModels
{
    public class RewardsDashboardViewModel
    {
        public int Balance { get; set; }
        public List<PointsTransactionDto> History { get; set; } = new();
        public List<RewardRuleDto> AvailableRewards { get; set; } = new();
    }
}