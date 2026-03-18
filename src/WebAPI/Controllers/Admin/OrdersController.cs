using Core.Application.Interfaces;
using Core.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers.Admin
{
    [ApiController]
    [Route("api/admin/[controller]")]
    [Authorize(Policy = "Admin")]
    public class OrdersController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IOrderService _orderService;

        public OrdersController(IUnitOfWork unitOfWork, IOrderService orderService)
        {
            _unitOfWork = unitOfWork;
            _orderService = orderService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var orders = await _unitOfWork.Orders.GetAllAsync();
            return Ok(orders);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var order = await _unitOfWork.Orders.GetByIdAsync(id);
            if (order == null)
                return NotFound(new { message = "Đơn hàng không tồn tại" });
            return Ok(order);
        }

        [HttpPut("{id}/status")]
        public async Task<IActionResult> UpdateStatus(int id, [FromBody] UpdateOrderStatusDto dto)
        {
            // Lấy User ID từ Claims (giả sử ID = 1 nếu chưa login)
            int userId = int.TryParse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value, out var uid) ? uid : 1;
            
            var result = await _orderService.UpdateOrderStatusAsync(id, dto.Status, userId);
            
            if (!result)
                return NotFound(new { message = "Không thể cập nhật trạng thái đơn hàng" });

            return Ok(new { success = true, message = $"Đã cập nhật trạng thái thành '{dto.Status}'" });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var order = await _unitOfWork.Orders.GetByIdAsync(id);
            if (order == null)
                return NotFound(new { message = "Đơn hàng không tồn tại" });

            await _unitOfWork.Orders.DeleteAsync(order);
            await _unitOfWork.CompleteAsync();

            return Ok(new { success = true, message = "Xóa đơn hàng thành công" });
        }
    }

    public class UpdateOrderStatusDto
    {
        public string Status { get; set; } = string.Empty;
    }
}
