using AutoMapper;
using Core.Application.DTOs;
using Core.Application.Interfaces;
using Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Core.Application.Services
{

    public class OrderService : IOrderService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public OrderService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<OrderDto> CreateOrderAsync(int userId, CreateOrderDto request)
        {
             // 1. Validate (Simple check)
            if (request.Items == null || request.Items.Count == 0) 
                throw new Exception("Cart is empty");

            // 2. Create Order
            var order = new Order
            {
                UserId = userId,
                CreatedAt = DateTime.UtcNow,
                Status = "New",
                ShippingAddress = request.ShippingAddress,
                PaymentMethod = request.PaymentMethod
            };

            decimal calculatedTotal = 0;

            // 3. Process Items
            foreach (var itemDto in request.Items)
            {
                var product = await _unitOfWork.Products.GetByIdAsync(itemDto.ProductId);
                if (product == null) throw new Exception($"Product {itemDto.ProductId} not found");

                var orderDetail = new OrderDetail
                {
                    ProductId = product.Id,
                    ProductName = product.Name,
                    Quantity = itemDto.Quantity,
                    UnitPrice = product.BasePrice
                };

                decimal itemTotal = product.BasePrice;

                // Process Options (Assuming we have a way to fetch them cheaply, or use UnitOfWork to get by Ids)
                if (itemDto.OptionValueIds != null)
                {
                    foreach (var optId in itemDto.OptionValueIds)
                    {
                        var optionVal = await _unitOfWork.ProductOptionValues.GetByIdAsync(optId);
                        if (optionVal != null)
                        {
                            orderDetail.Options.Add(new OrderDetailOption 
                            { 
                                OptionValueId = optId,
                                OptionName = optionVal.ValueName,
                                PriceAdjustment = optionVal.PriceAdjustment
                            });
                            itemTotal += optionVal.PriceAdjustment;
                        }
                    }
                }
                
                orderDetail.TotalPrice = itemTotal * itemDto.Quantity;
                calculatedTotal += orderDetail.TotalPrice;
                
                order.OrderDetails.Add(orderDetail);
            }

            order.TotalAmount = calculatedTotal;

            // 4. Save
            await _unitOfWork.Orders.AddAsync(order);
            await _unitOfWork.CompleteAsync();

            return _mapper.Map<OrderDto>(order);
        }

        public async Task<IReadOnlyList<OrderDto>> GetUserOrdersAsync(int userId)
        {
            var orders = await _unitOfWork.Orders.GetAsync(o => o.UserId == userId);
            return _mapper.Map<IReadOnlyList<OrderDto>>(orders);
        }

        public async Task<bool> UpdateOrderStatusAsync(int orderId, string newStatus, int processedByUserId)
        {
            var order = await _unitOfWork.Orders.GetByIdAsync(orderId);
            if (order == null) return false;

            var oldStatus = order.Status;
            if (oldStatus == newStatus) return true;

            var user = await _unitOfWork.Users.GetByIdAsync(processedByUserId);
            var userName = user?.FullName ?? "Unknown";

            order.Status = newStatus;
            order.UpdatedAt = DateTime.UtcNow;

            await _unitOfWork.Orders.UpdateAsync(order);

            // Log activity
            try
            {
                await _unitOfWork.AuditLogs.AddAsync(new AuditLog
                {
                    UserId = processedByUserId,
                    UserName = userName,
                    Action = "UpdateOrderStatus",
                    EntityName = "Order",
                    EntityId = orderId,
                    OldValue = oldStatus,
                    NewValue = newStatus,
                    CreatedAt = DateTime.UtcNow,
                    Note = $"Thay đổi trạng thái đơn hàng #{orderId} từ {oldStatus} sang {newStatus}"
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR] Không thể ghi AuditLog cho Order: {ex.Message}");
                // Ném lỗi để báo cho Controller biết việc phục vụ Audit Log thất bại (theo yêu cầu nghiệp vụ)
                throw new Exception($"Lỗi khi ghi lịch sử hệ thống: {ex.Message}", ex);
            }

            return await _unitOfWork.CompleteAsync() > 0;
        }
    }
}
