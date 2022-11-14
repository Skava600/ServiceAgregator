using Microsoft.EntityFrameworkCore;
using ServiceAggregator.Entities;

namespace ServiceAggregator.Data
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<Account> Accounts { get; set; }
        public DbSet<Doer> Doers { get; set; }
        public DbSet<DoerWorkSection> DoerWorkSections { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<Order> Orders { get; set; }    
        public DbSet<WorkSection> WorkSections { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

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
        }
    }
}
