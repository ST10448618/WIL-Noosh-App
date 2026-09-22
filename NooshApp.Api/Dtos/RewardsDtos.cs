namespace NooshApp.Api.Dtos
{
    public class PointsResultDto
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public int Balance { get; set; }
    }

    public class GenerateQrRequestDto { public string? FullName { get; set; } }
    public class ScanRequestDto { public string Token { get; set; } = string.Empty; public decimal AmountPaid { get; set; } }
    public class SubmitReceiptRequestDto
    {
        public string? FullName { get; set; }
        public string ReceiptReference { get; set; } = string.Empty;
        public decimal AmountPaid { get; set; }
        public DateOnly PurchaseDate { get; set; }
    }
    public class StaffRedeemRequestDto { 
        public string Email { get; set; } = string.Empty; 
        public int RewardRuleId { get; set; } 
        
    }
    public class CreateRewardRuleRequestDto
    {
        public string Name { get; set; } = string.Empty;
        public int PointsRequired { get; set; }
        public string RewardDescription { get; set; } = string.Empty;
    }
    public class UpdateRewardRuleRequestDto
    {
        public string Name { get; set; } = string.Empty;
        public int PointsRequired { get; set; }
        public string RewardDescription { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        public int DisplayOrder { get; set; }
    }
    public class UpdateSettingsRequestDto { public decimal PointsPerRand { get; set; } }
}