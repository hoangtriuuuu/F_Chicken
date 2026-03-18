using Core.Domain.Common;

namespace Core.Domain.Entities
{
    public class OrderDetailOption : BaseEntity
    {
        public int OrderDetailId { get; set; }
        public int OptionValueId { get; set; }
        
        // Snapshot data
        public string OptionName { get; set; }
        public decimal PriceAdjustment { get; set; }

        // Navigation
        public OrderDetail OrderDetail { get; set; }
        public ProductOptionValue OptionValue { get; set; }
    }
}
