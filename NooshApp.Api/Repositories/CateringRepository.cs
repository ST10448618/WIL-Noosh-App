using Microsoft.EntityFrameworkCore;
using NooshApp.Api.Data;
using NooshApp.Api.Models;
using NooshApp.Api.Repositories.Interfaces;

namespace NooshApp.Api.Repositories
{
    public class CateringRepository : ICateringRepository
    {
        private readonly ApplicationDbContext _context;
        public CateringRepository(ApplicationDbContext context) { _context = context; }

        public async Task AddAsync(CateringRequest request)
        {
            await _context.CateringRequests.AddAsync(request);
            await _context.SaveChangesAsync();
        }

        public async Task<CateringRequest?> GetByIdAsync(int id) =>
            await _context.CateringRequests.FindAsync(id);
    }
}