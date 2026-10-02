namespace NooshApp.Api.Dtos
{
    public class CateringRequestCreateDto
    {
        public string FullName { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public DateTime EventDate { get; set; }
        public int GuestCount { get; set; }
        public string EventLocation { get; set; } = string.Empty;
        public string? AdditionalNotes { get; set; }
    }

    public class CateringRequestDto
    {
        public int Id { get; set; }
        public string FullName { get; set; } = string.Empty;
        public DateTime EventDate { get; set; }
        public int GuestCount { get; set; }
        public string EventLocation { get; set; } = string.Empty;
        public string? AdditionalNotes { get; set; }
        public string Status { get; set; } = string.Empty;
        public DateTime SubmittedAt { get; set; }
    }
}