using Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Infrastructure.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<ProductOptionGroup> ProductOptionGroups { get; set; }
        public DbSet<ProductOptionValue> ProductOptionValues { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
        public DbSet<OrderDetailOption> OrderDetailOptions { get; set; }
        public DbSet<Voucher> Vouchers { get; set; }
        public DbSet<UserClaim> UserClaims { get; set; }
        public DbSet<ComboDetail> ComboDetails { get; set; }
        public DbSet<AuditLog> AuditLogs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            
            // Apply configurations from assembly
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            modelBuilder.Entity<ComboDetail>()
                .HasOne(cd => cd.Combo)
                .WithMany(p => p.ComboItems)
                .HasForeignKey(cd => cd.ComboId)
                .OnDelete(DeleteBehavior.Cascade); // Deleting Combo deletes items relationship

            modelBuilder.Entity<ComboDetail>()
                .HasOne(cd => cd.Product)
                .WithMany() // Product item doesn't need to know which combos it belongs to for now
                .HasForeignKey(cd => cd.ProductId)
                .OnDelete(DeleteBehavior.Restrict); // Cannot delete product if it is part of a combo
        }
    }
}
