namespace NooshApp.Web.Dtos
{
    public class ScanResultDto
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public int Balance { get; set; }
    }
}