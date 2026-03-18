using Core.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers.Admin
{
    [ApiController]
    [Route("api/admin/[controller]")]
    [Authorize(Policy = "Admin")]
    public class StatsController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public StatsController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        public async Task<IActionResult> GetStats()
        {
            try
            {
                // Fetch all data
                var products = await _unitOfWork.Products.GetAllAsync();
                var categories = await _unitOfWork.Categories.GetAllAsync();
                var orders = await _unitOfWork.Orders.GetAllAsync();

                // Calculate stats
                var productCount = products.Count;
                var categoryCount = categories.Count;
                var orderCount = orders.Count;
                
                // Calculate revenue from non-cancelled orders
                // Assuming "Cancelled" is the status for cancelled orders
                // If status is "Completed", we might want to count only completed ones.
                // For now, let's include all non-cancelled orders to show potential revenue too, 
                // or just Completed for realized revenue. Let's stick to Completed + Delivered for now or just all non-cancelled.
                // Based on previous code, status defaults to "New".
                // Let's sum up orders that are NOT Cancelled.
                var revenue = orders
                    .Where(o => o.Status != "Cancelled")
                    .Sum(o => o.TotalAmount);

                return Ok(new
                {
                    productCount,
                    categoryCount,
                    orderCount,
                    revenue
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Lỗi khi lấy dữ liệu thống kê", error = ex.Message });
            }
        }

        [HttpGet("revenue")]
        public async Task<IActionResult> GetRevenueStats()
        {
            try
            {
                var orders = await _unitOfWork.Orders.GetAllAsync();
                var last7Days = Enumerable.Range(0, 7)
                    .Select(i => DateTime.UtcNow.Date.AddDays(-i))
                    .OrderBy(d => d)
                    .ToList();

                var revenueData = last7Days.Select(date => new
                {
                    Date = date.ToString("dd/MM"),
                    Revenue = orders
                        .Where(o => o.CreatedAt.Date == date && o.Status != "Cancelled")
                        .Sum(o => o.TotalAmount)
                }).ToList();

                return Ok(revenueData);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Lỗi khi lấy dữ liệu doanh thu", error = ex.Message });
            }
        }
    }
}
