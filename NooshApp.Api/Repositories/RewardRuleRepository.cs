using Microsoft.EntityFrameworkCore;
using NooshApp.Api.Data;
using NooshApp.Api.Models;
using NooshApp.Api.Repositories.Interfaces;

namespace NooshApp.Api.Repositories
{
    public class RewardRuleRepository : IRewardRuleRepository
    {
        private readonly ApplicationDbContext _context;
        public RewardRuleRepository(ApplicationDbContext context) { _context = context; }

        public async Task<List<RewardRule>> GetActiveAsync() =>
            await _context.RewardRules.Where(r => r.IsActive && !r.IsDeleted)
                .OrderBy(r => r.DisplayOrder).ToListAsync();

        public async Task<List<RewardRule>> GetAllAsync() =>
            await _context.RewardRules.Where(r => !r.IsDeleted)
                .OrderBy(r => r.DisplayOrder).ToListAsync();

        public async Task<RewardRule?> GetByIdAsync(int id) => await _context.RewardRules.FindAsync(id);
        public async Task<RewardRule> CreateAsync(RewardRule rule)
        {
            await _context.RewardRules.AddAsync(rule);
            await _context.SaveChangesAsync();
            return rule;
        }

        public async Task UpdateAsync(RewardRule rule)
        {
            rule.UpdatedAt = DateTime.UtcNow;
            _context.RewardRules.Update(rule);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var rule = await _context.RewardRules.FindAsync(id);
            if (rule != null)
            {
                rule.IsDeleted = true;
                rule.UpdatedAt = DateTime.UtcNow;
                _context.RewardRules.Update(rule);
                await _context.SaveChangesAsync();
            }
        }
    }
}