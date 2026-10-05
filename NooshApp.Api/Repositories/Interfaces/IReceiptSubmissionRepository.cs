namespace NooshApp.Api.Repositories.Interfaces
{
    public interface IReceiptSubmissionRepository
    {
        Task<bool> ExistsAsync(string receiptReference, decimal amountPaid, DateOnly purchaseDate);
        Task AddAsync(Models.ReceiptSubmission submission);
    }
}