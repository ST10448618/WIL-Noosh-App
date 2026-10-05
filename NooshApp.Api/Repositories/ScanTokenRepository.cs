using Microsoft.EntityFrameworkCore;
using NooshApp.Api.Data;
using NooshApp.Api.Models;
using NooshApp.Api.Repositories.Interfaces;

namespace NooshApp.Api.Repositories
{
    public class ScanTokenRepository : IScanTokenRepository
    {
        private readonly ApplicationDbContext _context;
        public ScanTokenRepository(ApplicationDbContext context) { _context = context; }

        public async Task<ScanToken> CreateAsync(int customerId, TimeSpan validFor)
        {
            var token = new ScanToken
            {
                Token = Guid.NewGuid().ToString("N"),
                CustomerId = customerId,
                ExpiresAt = DateTime.UtcNow.Add(validFor)
            };
            await _context.ScanTokens.AddAsync(token);
            await _context.SaveChangesAsync();
            return token;
        }

        public async Task<ScanToken?> GetByTokenAsync(string token) =>
            await _context.ScanTokens.Include(t => t.Customer).FirstOrDefaultAsync(t => t.Token == token);

        public async Task MarkUsedAsync(ScanToken token)
        {
            token.IsUsed = true;
            _context.ScanTokens.Update(token);
            await _context.SaveChangesAsync();
        }
    }
}