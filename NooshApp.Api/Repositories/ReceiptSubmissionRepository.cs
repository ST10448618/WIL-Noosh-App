using Microsoft.EntityFrameworkCore;
using NooshApp.Api.Data;
using NooshApp.Api.Models;
using NooshApp.Api.Repositories.Interfaces;

namespace NooshApp.Api.Repositories
{
    public class ReceiptSubmissionRepository : IReceiptSubmissionRepository
    {
        private readonly ApplicationDbContext _context;
        public ReceiptSubmissionRepository(ApplicationDbContext context) { _context = context; }

        public async Task<bool> ExistsAsync(string receiptReference, decimal amountPaid, DateOnly purchaseDate) =>
            await _context.ReceiptSubmissions.AnyAsync(r =>
                r.ReceiptReference == receiptReference && r.AmountPaid == amountPaid && r.PurchaseDate == purchaseDate);

        public async Task AddAsync(ReceiptSubmission submission)
        {
            await _context.ReceiptSubmissions.AddAsync(submission);
            await _context.SaveChangesAsync();
        }
    }
}