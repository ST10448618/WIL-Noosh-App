using NooshApp.Api.Models;

namespace NooshApp.Api.Repositories.Interfaces
{
    public interface IJobApplicationRepository
    {
        Task AddAsync(JobApplication application);
    }
}