using NooshApp.Web.ViewModels;

namespace NooshApp.Web.Services
{
    public class CareersApplyResult
    {
        public bool Success { get; set; }
        public int Id { get; set; }
        public string? ErrorMessage { get; set; }
    }

    public interface ICareersApiClient
    {
        Task<CareersApplyResult> SubmitApplicationAsync(CareerApplicationViewModel model);
    }
}