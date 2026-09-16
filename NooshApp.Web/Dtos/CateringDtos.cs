namespace NooshApp.Web.Dtos
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
}