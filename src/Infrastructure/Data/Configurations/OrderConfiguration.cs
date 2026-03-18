using Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations
{
    public class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.Property(o => o.TotalAmount).HasColumnType("decimal(18,2)");
            builder.HasMany(o => o.OrderDetails)
                   .WithOne(d => d.Order)
                   .HasForeignKey(d => d.OrderId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }

    public class OrderDetailConfiguration : IEntityTypeConfiguration<OrderDetail>
    {
        public void Configure(EntityTypeBuilder<OrderDetail> builder)
        {
             // OrderDetails are configured in OrderConfiguration using OwnsMany or here using HasOne
             // For simplicity, defining relationships explicitly
             builder.HasOne(d => d.Order)
                    .WithMany(o => o.OrderDetails)
                    .HasForeignKey(d => d.OrderId)
                    .OnDelete(DeleteBehavior.Cascade);

             builder.Property(d => d.UnitPrice).HasColumnType("decimal(18,2)");
             builder.Property(d => d.TotalPrice).HasColumnType("decimal(18,2)");
        }
    }
    
    public class OrderDetailOptionConfiguration : IEntityTypeConfiguration<OrderDetailOption>
    {
        public void Configure(EntityTypeBuilder<OrderDetailOption> builder)
        {
             builder.Property(o => o.PriceAdjustment).HasColumnType("decimal(18,2)");
             
             builder.HasOne(o => o.OrderDetail)
                    .WithMany(d => d.Options)
                    .HasForeignKey(o => o.OrderDetailId)
                    .OnDelete(DeleteBehavior.Cascade);
                    
             // Use Restrict to prevent multiple cascade paths
             builder.HasOne(o => o.OptionValue)
                    .WithMany()
                    .HasForeignKey(o => o.OptionValueId)
                    .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
