namespace NooshApp.Web.Dtos
{
    public class RewardRuleDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int PointsRequired { get; set; }
        public string RewardDescription { get; set; } = string.Empty;
    }

    public class PointsTransactionDto
    {
        public int Amount { get; set; }
        public string Description { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
    }

    public class BalanceResponseDto
    {
        public int Balance { get; set; }
        public List<PointsTransactionDto> History { get; set; } = new();
    }

    public class GenerateQrResponseDto
    {
        public string Token { get; set; } = string.Empty;
        public string QrImageDataUrl { get; set; } = string.Empty;
    }
}