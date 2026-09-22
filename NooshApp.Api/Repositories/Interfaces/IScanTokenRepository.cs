using NooshApp.Api.Models;
namespace NooshApp.Api.Repositories.Interfaces
{
    public interface IScanTokenRepository
    {
        Task<ScanToken> CreateAsync(int customerId, TimeSpan validFor);
        Task<ScanToken?> GetByTokenAsync(string token);
        Task MarkUsedAsync(ScanToken token);
    }
}