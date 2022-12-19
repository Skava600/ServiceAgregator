using ServiceAggregator.Entities;
using ORM;
using Microsoft.EntityFrameworkCore;

namespace ServiceAggregator.Data
{
    public class ApplicationDbContext : DbContract
    {
        public DbTable<Account> Accounts { get; set; }
        public DbTable<Doer> Doers { get; set; }
        public DbTable<DoerSection> DoerSections { get; set; }
        public DbTable<Customer> Customers { get; set; }
        public DbTable<CustomerReview> CustomerReviews { get; set; }
        public DbTable<DoerReview> DoerReviews { get; set; }
        public DbTable<Order> Orders { get; set; }    
        public DbTable<Section> Sections { get; set; }
        public DbTable<Category> Categorys { get; set; }
        public DbTable<OrderResponse> OrderResponses { get; set; }
        public DbTable<BannedCustomer> BannedCustomers { get; set; }
        public DbTable<BannedDoer> BannedDoers { get; set; }
        public DbTable<DeletedOrder> DeletedOrders { get; set; }
        public DbTable<BannedAccount> BannedAccounts { get; set; }
        public ApplicationDbContext(string connectionString)
       : base(connectionString)
        {
        }
        /*
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Account>()
                .HasOne<Doer>(a => a.Service)
                .WithOne(s => s.Account)
                .HasForeignKey<Doer>(s => s.AccountId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Doer>()
                .HasMany<Review>(s => s.Reviews)
                .WithOne(r => r.ReviewedDoer)
                .HasForeignKey(r => r.DoerId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Account>()
                .HasMany<Review>()
                .WithOne(r => r.AccountAuthor)
                .HasForeignKey(r => r.AccountAuthorId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<DoerWorkSection>()
                .HasKey(sw => new { sw.WorkSectionId, sw.DoerId });

            modelBuilder.Entity<DoerWorkSection>()
                .HasOne<Doer>(sw => sw.Doer)
                .WithMany(s => s.ServiceWorkSections)
                .HasForeignKey(sw => sw.DoerId);

            modelBuilder.Entity<DoerWorkSection>()
                .HasOne<WorkSection>(sw => sw.WorkSection)
                .WithMany(ws => ws.DoerWorkSections)
                .HasForeignKey(sw => sw.WorkSectionId);

            modelBuilder.Entity<Account>()
                .HasOne<Customer>(a => a.Customer)
                .WithOne(c => c.Account)
                .HasForeignKey<Customer>(c => c.AccountId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Customer>()
                .HasMany<Review>(c => c.Reviews)
                .WithOne(r => r.ReviewedCustomer)
                .HasForeignKey(r => r.CustomerId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<WorkSection>()
                .HasMany<Order>(ws => ws.Orders)
                .WithOne(o => o.WorkSection)
                .HasForeignKey(o => o.WorkSectionId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Customer>()
                .HasMany<Order>(c => c.Orders)
                .WithOne(o => o.Customer)
                .HasForeignKey(o => o.CustomerId)
                .OnDelete(DeleteBehavior.Cascade);
        }*/
    }
}
