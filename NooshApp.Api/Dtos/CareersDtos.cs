namespace NooshApp.Api.Dtos
{
    ///What the API sends back after a successful submission
    public class JobApplicationDto
    {
        public int Id { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string DesiredPosition { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public DateTime SubmittedAt { get; set; }
    }
}