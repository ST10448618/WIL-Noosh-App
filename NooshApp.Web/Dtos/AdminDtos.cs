namespace NooshApp.Web.Dtos
{
    public class AdminRewardRuleDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int PointsRequired { get; set; }
        public string RewardDescription { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        public int DisplayOrder { get; set; }
    }

    public class AppSettingsDto
    {
        public decimal PointsPerRand { get; set; }
    }
}