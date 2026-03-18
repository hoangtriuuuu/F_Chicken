using Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace Infrastructure.Data.Configurations
{
    public class SeedDataConfiguration : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.HasData(
                new Category { Id = 1, Name = "Gà Rán", Description = "Gà rán giòn tan", IsActive = true, CreatedAt = new DateTime(2026, 1, 1) },
                new Category { Id = 2, Name = "Combo", Description = "Combo tiết kiệm", IsActive = true, CreatedAt = new DateTime(2026, 1, 1) },
                new Category { Id = 3, Name = "Nước Uống", Description = "Đồ uống giải khát", IsActive = true, CreatedAt = new DateTime(2026, 1, 1) },
                new Category { Id = 4, Name = "Ăn Kèm", Description = "Món ăn kèm", IsActive = true, CreatedAt = new DateTime(2026, 1, 1) }
            );
        }
    }

    public class ProductSeedConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.HasData(
                // Gà Rán
                new Product { Id = 1, Name = "Gà Rán Giòn (1 miếng)", Description = "Miếng gà rán giòn tan, thơm ngon", BasePrice = 35000, ImageUrl = "https://placehold.co/300x200?text=Ga+Ran", IsCombo = false, IsActive = true, CategoryId = 1, CreatedAt = new DateTime(2026, 1, 1) },
                new Product { Id = 2, Name = "Gà Rán Giòn (2 miếng)", Description = "2 miếng gà rán giòn tan", BasePrice = 65000, ImageUrl = "https://placehold.co/300x200?text=Ga+Ran+2", IsCombo = false, IsActive = true, CategoryId = 1, CreatedAt = new DateTime(2026, 1, 1) },
                new Product { Id = 3, Name = "Gà Sốt Cay (1 miếng)", Description = "Miếng gà rán sốt cay đặc biệt", BasePrice = 40000, ImageUrl = "https://placehold.co/300x200?text=Ga+Cay", IsCombo = false, IsActive = true, CategoryId = 1, CreatedAt = new DateTime(2026, 1, 1) },
                new Product { Id = 4, Name = "Cánh Gà Chiên (4 miếng)", Description = "4 cánh gà chiên giòn", BasePrice = 55000, ImageUrl = "https://placehold.co/300x200?text=Canh+Ga", IsCombo = false, IsActive = true, CategoryId = 1, CreatedAt = new DateTime(2026, 1, 1) },
                
                // Combo
                new Product { Id = 5, Name = "Combo 1 Người", Description = "1 Gà rán + 1 Khoai tây + 1 Pepsi", BasePrice = 79000, ImageUrl = "https://placehold.co/300x200?text=Combo+1", IsCombo = true, IsActive = true, CategoryId = 2, CreatedAt = new DateTime(2026, 1, 1) },
                new Product { Id = 6, Name = "Combo 2 Người", Description = "2 Gà rán + 1 Khoai tây lớn + 2 Pepsi", BasePrice = 149000, ImageUrl = "https://placehold.co/300x200?text=Combo+2", IsCombo = true, IsActive = true, CategoryId = 2, CreatedAt = new DateTime(2026, 1, 1) },
                new Product { Id = 7, Name = "Combo Gia Đình", Description = "4 Gà rán + 2 Khoai tây + 4 Pepsi + 1 Salad", BasePrice = 299000, ImageUrl = "https://placehold.co/300x200?text=Combo+GD", IsCombo = true, IsActive = true, CategoryId = 2, CreatedAt = new DateTime(2026, 1, 1) },
                
                // Nước uống
                new Product { Id = 8, Name = "Pepsi", Description = "Pepsi lon 330ml", BasePrice = 15000, ImageUrl = "https://placehold.co/300x200?text=Pepsi", IsCombo = false, IsActive = true, CategoryId = 3, CreatedAt = new DateTime(2026, 1, 1) },
                new Product { Id = 9, Name = "7Up", Description = "7Up lon 330ml", BasePrice = 15000, ImageUrl = "https://placehold.co/300x200?text=7Up", IsCombo = false, IsActive = true, CategoryId = 3, CreatedAt = new DateTime(2026, 1, 1) },
                new Product { Id = 10, Name = "Trà Đào", Description = "Trà đào thơm mát", BasePrice = 25000, ImageUrl = "https://placehold.co/300x200?text=Tra+Dao", IsCombo = false, IsActive = true, CategoryId = 3, CreatedAt = new DateTime(2026, 1, 1) },
                
                // Ăn kèm
                new Product { Id = 11, Name = "Khoai Tây Chiên (M)", Description = "Khoai tây chiên cỡ vừa", BasePrice = 25000, ImageUrl = "https://placehold.co/300x200?text=Khoai+M", IsCombo = false, IsActive = true, CategoryId = 4, CreatedAt = new DateTime(2026, 1, 1) },
                new Product { Id = 12, Name = "Khoai Tây Chiên (L)", Description = "Khoai tây chiên cỡ lớn", BasePrice = 35000, ImageUrl = "https://placehold.co/300x200?text=Khoai+L", IsCombo = false, IsActive = true, CategoryId = 4, CreatedAt = new DateTime(2026, 1, 1) },
                new Product { Id = 13, Name = "Salad Trộn", Description = "Salad rau củ tươi", BasePrice = 30000, ImageUrl = "https://placehold.co/300x200?text=Salad", IsCombo = false, IsActive = true, CategoryId = 4, CreatedAt = new DateTime(2026, 1, 1) }
            );
        }
    }

    public class ProductOptionGroupSeedConfiguration : IEntityTypeConfiguration<ProductOptionGroup>
    {
        public void Configure(EntityTypeBuilder<ProductOptionGroup> builder)
        {
            builder.HasData(
                // Options for Combo 1 Người (Product 5)
                new ProductOptionGroup { Id = 1, ProductId = 5, GroupName = "Chọn loại gà", IsRequired = true, AllowMultiple = false, CreatedAt = new DateTime(2026, 1, 1) },
                new ProductOptionGroup { Id = 2, ProductId = 5, GroupName = "Thêm sốt", IsRequired = false, AllowMultiple = true, CreatedAt = new DateTime(2026, 1, 1) },
                
                // Options for Combo 2 Người (Product 6)
                new ProductOptionGroup { Id = 3, ProductId = 6, GroupName = "Chọn loại gà", IsRequired = true, AllowMultiple = false, CreatedAt = new DateTime(2026, 1, 1) },
                new ProductOptionGroup { Id = 4, ProductId = 6, GroupName = "Thêm sốt", IsRequired = false, AllowMultiple = true, CreatedAt = new DateTime(2026, 1, 1) },
                
                // Options for Gà Rán đơn (Product 1)
                new ProductOptionGroup { Id = 5, ProductId = 1, GroupName = "Thêm sốt", IsRequired = false, AllowMultiple = true, CreatedAt = new DateTime(2026, 1, 1) }
            );
        }
    }

    public class ProductOptionValueSeedConfiguration : IEntityTypeConfiguration<ProductOptionValue>
    {
        public void Configure(EntityTypeBuilder<ProductOptionValue> builder)
        {
            builder.HasData(
                // Loại gà for Combo 1 (Group 1)
                new ProductOptionValue { Id = 1, OptionGroupId = 1, ValueName = "Gà Rán Giòn", PriceAdjustment = 0, CreatedAt = new DateTime(2026, 1, 1) },
                new ProductOptionValue { Id = 2, OptionGroupId = 1, ValueName = "Gà Sốt Cay", PriceAdjustment = 5000, CreatedAt = new DateTime(2026, 1, 1) },
                new ProductOptionValue { Id = 3, OptionGroupId = 1, ValueName = "Gà Sốt Mật Ong", PriceAdjustment = 8000, CreatedAt = new DateTime(2026, 1, 1) },
                
                // Sốt for Combo 1 (Group 2)
                new ProductOptionValue { Id = 4, OptionGroupId = 2, ValueName = "Sốt Tương Ớt", PriceAdjustment = 0, CreatedAt = new DateTime(2026, 1, 1) },
                new ProductOptionValue { Id = 5, OptionGroupId = 2, ValueName = "Sốt Mayonnaise", PriceAdjustment = 5000, CreatedAt = new DateTime(2026, 1, 1) },
                new ProductOptionValue { Id = 6, OptionGroupId = 2, ValueName = "Sốt BBQ", PriceAdjustment = 5000, CreatedAt = new DateTime(2026, 1, 1) },
                
                // Loại gà for Combo 2 (Group 3)
                new ProductOptionValue { Id = 7, OptionGroupId = 3, ValueName = "Gà Rán Giòn", PriceAdjustment = 0, CreatedAt = new DateTime(2026, 1, 1) },
                new ProductOptionValue { Id = 8, OptionGroupId = 3, ValueName = "Gà Sốt Cay", PriceAdjustment = 10000, CreatedAt = new DateTime(2026, 1, 1) },
                new ProductOptionValue { Id = 9, OptionGroupId = 3, ValueName = "Gà Sốt Mật Ong", PriceAdjustment = 15000, CreatedAt = new DateTime(2026, 1, 1) },
                
                // Sốt for Combo 2 (Group 4)
                new ProductOptionValue { Id = 10, OptionGroupId = 4, ValueName = "Sốt Tương Ớt", PriceAdjustment = 0, CreatedAt = new DateTime(2026, 1, 1) },
                new ProductOptionValue { Id = 11, OptionGroupId = 4, ValueName = "Sốt Mayonnaise", PriceAdjustment = 5000, CreatedAt = new DateTime(2026, 1, 1) },
                new ProductOptionValue { Id = 12, OptionGroupId = 4, ValueName = "Sốt BBQ", PriceAdjustment = 5000, CreatedAt = new DateTime(2026, 1, 1) },
                
                // Sốt for Gà Rán đơn (Group 5)
                new ProductOptionValue { Id = 13, OptionGroupId = 5, ValueName = "Sốt Tương Ớt", PriceAdjustment = 0, CreatedAt = new DateTime(2026, 1, 1) },
                new ProductOptionValue { Id = 14, OptionGroupId = 5, ValueName = "Sốt Mayonnaise", PriceAdjustment = 3000, CreatedAt = new DateTime(2026, 1, 1) },
                new ProductOptionValue { Id = 15, OptionGroupId = 5, ValueName = "Sốt BBQ", PriceAdjustment = 3000, CreatedAt = new DateTime(2026, 1, 1) }
            );
        }
    }
}
