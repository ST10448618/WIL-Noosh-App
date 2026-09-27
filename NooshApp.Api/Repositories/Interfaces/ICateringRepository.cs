using NooshApp.Api.Models;

namespace NooshApp.Api.Repositories.Interfaces
{
    public interface ICateringRepository
    {
        Task AddAsync(CateringRequest request);
        Task<CateringRequest?> GetByIdAsync(int id);
    }
}