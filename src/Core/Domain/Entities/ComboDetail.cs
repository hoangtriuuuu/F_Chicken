using Core.Domain.Common;

namespace Core.Domain.Entities
{
    public class ComboDetail : BaseEntity
    {
        public int ComboId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; } = 1;

        // Navigation
        public Product Combo { get; set; }
        public Product Product { get; set; }
    }
}
