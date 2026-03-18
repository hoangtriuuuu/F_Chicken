using Core.Domain.Common;

namespace Core.Domain.Entities
{
    public class ProductOptionValue : BaseEntity
    {
        public int OptionGroupId { get; set; }
        public string ValueName { get; set; }
        public decimal PriceAdjustment { get; set; }

        // Navigation
        public ProductOptionGroup OptionGroup { get; set; }
    }
}
