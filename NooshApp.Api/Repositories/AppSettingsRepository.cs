using Microsoft.EntityFrameworkCore;
using NooshApp.Api.Data;
using NooshApp.Api.Models;
using NooshApp.Api.Repositories.Interfaces;

namespace NooshApp.Api.Repositories
{
    public class AppSettingsRepository : IAppSettingsRepository
    {
        private readonly ApplicationDbContext _context;
        public AppSettingsRepository(ApplicationDbContext context) { _context = context; }

        public async Task<AppSettings> GetAsync() => await _context.AppSettings.FirstAsync(s => s.Id == 1);

        public async Task UpdateAsync(decimal pointsPerRand)
        {
            var settings = await GetAsync();
            settings.PointsPerRand = pointsPerRand;
            settings.UpdatedAt = DateTime.UtcNow;
            _context.AppSettings.Update(settings);
            await _context.SaveChangesAsync();
        }
    }
}