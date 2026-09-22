using Microsoft.EntityFrameworkCore;
using NooshApp.Api.Data;
using NooshApp.Api.Models;
using NooshApp.Api.Repositories.Interfaces;

namespace NooshApp.Api.Repositories
{
    public class PointsRepository : IPointsRepository
    {
        private readonly ApplicationDbContext _context;
        public PointsRepository(ApplicationDbContext context) { _context = context; }

        public async Task<int> GetBalanceAsync(int customerId) =>
            await _context.PointsTransactions.Where(t => t.CustomerId == customerId).SumAsync(t => t.Amount);

        public async Task<List<PointsTransaction>> GetHistoryAsync(int customerId) =>
            await _context.PointsTransactions.Where(t => t.CustomerId == customerId)
                .OrderByDescending(t => t.CreatedAt).ToListAsync();

        public async Task AddTransactionAsync(PointsTransaction transaction)
        {
            await _context.PointsTransactions.AddAsync(transaction);
            await _context.SaveChangesAsync();
        }
    }
}