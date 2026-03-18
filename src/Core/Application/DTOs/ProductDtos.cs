using System.Collections.Generic;

namespace Core.Application.DTOs
{
    public class ProductDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal BasePrice { get; set; }
        public string ImageUrl { get; set; }
        public bool IsCombo { get; set; }
        public string CategoryName { get; set; }
        public int CategoryId { get; set; }
        
        public List<ProductOptionGroupDto> OptionGroups { get; set; }
        public List<ComboItemDto> ComboItems { get; set; }
    }

    public class ProductOptionGroupDto
    {
        public int Id { get; set; }
        public string GroupName { get; set; }
        public bool IsRequired { get; set; }
        public bool AllowMultiple { get; set; }
        public List<ProductOptionValueDto> OptionValues { get; set; }
    }

    public class ProductOptionValueDto
    {
        public int Id { get; set; }
        public string ValueName { get; set; }
        public decimal PriceAdjustment { get; set; }
    }

    public class CreateProductDto
    {
        public string Name { get; set; }
        public string? Description { get; set; }
        public decimal BasePrice { get; set; }
        public string? ImageUrl { get; set; }
        public bool IsCombo { get; set; }
        public int CategoryId { get; set; }
        public List<ComboItemDto> ComboItems { get; set; } = new List<ComboItemDto>();
    }

    public class ComboItemDto
    {
        public int ProductId { get; set; }
        public string? ProductName { get; set; }
        public string? ImageUrl { get; set; }
        public int Quantity { get; set; }
    }
}
