using Core.Domain.Common;
using System;
using System.Collections.Generic;

namespace Core.Domain.Entities
{
    public class Order : BaseEntity
    {
        public int UserId { get; set; } // Reference to User (Identity)
        public decimal TotalAmount { get; set; }
        public string Status { get; set; } = "New"; // New, Preparing, Delivering, Completed, Cancelled
        public string ShippingAddress { get; set; }
        public string PaymentMethod { get; set; }

        // Navigation
        public User? User { get; set; }
        public ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();
    }
}
