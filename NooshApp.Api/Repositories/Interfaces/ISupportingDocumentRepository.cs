using NooshApp.Api.Models;

namespace NooshApp.Api.Repositories.Interfaces
{
    public interface ISupportingDocumentRepository
    {
        //Task indicates that this is an asynchronous operation. When implemented, 
        //it will run without blocking the main server thread.
        Task AddRangeAsync(List<SupportingDocument> documents);
        //Add range means multiple records at once.

    }
}