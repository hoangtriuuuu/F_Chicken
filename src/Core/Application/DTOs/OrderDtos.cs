using System;
using System.Collections.Generic;

namespace Core.Application.DTOs
{
    public class OrderDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public decimal TotalAmount { get; set; }
        public string Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public string ShippingAddress { get; set; }
        
        public List<OrderDetailDto> OrderDetails { get; set; }
    }

    public class OrderDetailDto
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal TotalPrice { get; set; }
        
        public List<OrderDetailOptionDto> Options { get; set; }
    }

    public class OrderDetailOptionDto
    {
        public string OptionName { get; set; }
        public decimal PriceAdjustment { get; set; }
    }

    public class CreateOrderDto
    {
        public string ShippingAddress { get; set; }
        public string PaymentMethod { get; set; }
        public List<CartItemDto> Items { get; set; }
    }

    public class CartItemDto
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public List<int> OptionValueIds { get; set; } // List of selected option value IDs
    }
}
