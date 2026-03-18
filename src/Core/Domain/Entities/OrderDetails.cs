using Core.Domain.Common;
using System.Collections.Generic;

namespace Core.Domain.Entities
{
    public class OrderDetail : BaseEntity
    {
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        
        // Snapshot data
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; } // Base price at time of order
        public decimal TotalPrice { get; set; } // (UnitPrice + Options) * Quantity

        // Navigation
        public Order Order { get; set; }
        public Product Product { get; set; }
        public ICollection<OrderDetailOption> Options { get; set; } = new List<OrderDetailOption>();
    }

}
