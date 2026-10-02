using NooshApp.Api.Models;
namespace NooshApp.Api.Repositories.Interfaces
{
    public interface IAppSettingsRepository
    {
        Task<AppSettings> GetAsync();
        Task UpdateAsync(decimal pointsPerRand);
    }
}