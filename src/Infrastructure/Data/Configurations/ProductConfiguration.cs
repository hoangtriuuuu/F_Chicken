using Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations
{
    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.Property(p => p.Name).IsRequired().HasMaxLength(100);
            builder.Property(p => p.BasePrice).HasColumnType("decimal(18,2)");
            
            builder.HasOne(p => p.Category)
                   .WithMany(c => c.Products)
                   .HasForeignKey(p => p.CategoryId);
        }
    }

    public class ProductOptionGroupConfiguration : IEntityTypeConfiguration<ProductOptionGroup>
    {
        public void Configure(EntityTypeBuilder<ProductOptionGroup> builder)
        {
            builder.HasOne(g => g.Product)
                   .WithMany(p => p.OptionGroups)
                   .HasForeignKey(g => g.ProductId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }

    public class ProductOptionValueConfiguration : IEntityTypeConfiguration<ProductOptionValue>
    {
        public void Configure(EntityTypeBuilder<ProductOptionValue> builder)
        {
            builder.Property(v => v.PriceAdjustment).HasColumnType("decimal(18,2)");

            builder.HasOne(v => v.OptionGroup)
                   .WithMany(g => g.OptionValues)
                   .HasForeignKey(v => v.OptionGroupId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
