using NooshApp.Api.Models;
namespace NooshApp.Api.Repositories.Interfaces
{
    public interface ICustomerRepository
    {
        Task<Customer?> GetByEmailAsync(string email);
        Task<Customer> CreateAsync(string email, string? fullName);
    }
}