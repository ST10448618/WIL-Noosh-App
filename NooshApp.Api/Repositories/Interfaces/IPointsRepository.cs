using NooshApp.Api.Models;
namespace NooshApp.Api.Repositories.Interfaces
{
    public interface IPointsRepository
    {
        Task<int> GetBalanceAsync(int customerId);
        Task<List<PointsTransaction>> GetHistoryAsync(int customerId);
        Task AddTransactionAsync(PointsTransaction transaction);
    }
}