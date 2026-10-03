using Microsoft.EntityFrameworkCore;
using NooshApp.Api.Models;

namespace NooshApp.Api.Data
{
    /// <summary>
    /// The EF Core database session. Represents a connection to the database
    /// and exposes each entity as a queryable DbSet.
    /// </summary>
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<MenuItem> MenuItems { get; set; }
        public DbSet<CateringRequest> CateringRequests { get; set; }
        public DbSet<JobApplication> JobApplications { get; set; }

        public DbSet<Customer> Customers { get; set; }
        public DbSet<RewardRule> RewardRules { get; set; }
        public DbSet<PointsTransaction> PointsTransactions { get; set; }
        public DbSet<ScanToken> ScanTokens { get; set; }
        public DbSet<ReceiptSubmission> ReceiptSubmissions { get; set; }
        public DbSet<AppSettings> AppSettings { get; set; }
        public DbSet<SupportingDocument> SupportingDocuments { get; set; }
        public DbSet<Favourite> Favourites { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Customer>().HasIndex(c => c.Email).IsUnique();
            modelBuilder.Entity<ScanToken>().HasIndex(t => t.Token).IsUnique();
            modelBuilder.Entity<ReceiptSubmission>()
                .HasIndex(r => new { r.ReceiptReference, r.AmountPaid, r.PurchaseDate }).IsUnique();
            // inside OnModelCreating
            modelBuilder.Entity<Favourite>()
                .HasIndex(f => new { f.CustomerId, f.MenuItemId })
                .IsUnique();
        }   
    }
}