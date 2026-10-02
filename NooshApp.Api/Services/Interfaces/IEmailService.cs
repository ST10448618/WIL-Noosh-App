using NooshApp.Api.Models;

namespace NooshApp.Api.Services.Interfaces
{
    //When a candidate submits a job application, the business logic can call this interface to send an 
    //email notification to the hiring team, complete with all of the candidate's uploaded files attached.
    public interface IEmailService
    {
        Task SendCareerApplicationNotificationAsync(JobApplication application, List<string> attachmentPaths);
    }
}