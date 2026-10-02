using Microsoft.EntityFrameworkCore;
using NooshApp.Api.Data;
using NooshApp.Api.Models;
using NooshApp.Api.Repositories.Interfaces;

namespace NooshApp.Api.Repositories
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly ApplicationDbContext _context;
        public CustomerRepository(ApplicationDbContext context) { _context = context; }

        public async Task<Customer?> GetByEmailAsync(string email) =>
            await _context.Customers.FirstOrDefaultAsync(c => c.Email == email);

        public async Task<Customer> CreateAsync(string email, string? fullName)
        {
            var customer = new Customer { Email = email, FullName = fullName };
            await _context.Customers.AddAsync(customer);
            await _context.SaveChangesAsync();
            return customer;
        }
    }
}