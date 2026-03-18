using Core.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("api/vouchers")]
    public class PublicVouchersController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public PublicVouchersController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpPost("validate")]
        public async Task<IActionResult> ValidateVoucher([FromBody] ValidateVoucherRequest request)
        {
            var vouchers = await _unitOfWork.Vouchers.GetAllAsync();
            var voucher = vouchers.FirstOrDefault(v => 
                v.Code.ToUpper() == request.Code.ToUpper() && 
                !v.IsDeleted);

            if (voucher == null)
            {
                return BadRequest(new { message = "Mã voucher không tồn tại" });
            }

            if (!voucher.IsActive)
            {
                return BadRequest(new { message = "Voucher đã bị vô hiệu hóa" });
            }

            var now = DateTime.UtcNow;
            if (now < voucher.StartDate || now > voucher.EndDate)
            {
                return BadRequest(new { message = "Voucher đã hết hạn hoặc chưa có hiệu lực" });
            }

            if (voucher.UsageLimit > 0 && voucher.UsedCount >= voucher.UsageLimit)
            {
                return BadRequest(new { message = "Voucher đã hết lượt sử dụng" });
            }

            if (request.OrderAmount < voucher.MinOrderAmount)
            {
                return BadRequest(new { message = $"Đơn hàng tối thiểu {voucher.MinOrderAmount:N0}₫" });
            }

            // Calculate discount
            decimal discountAmount;
            if (voucher.DiscountType == "Percentage")
            {
                discountAmount = request.OrderAmount * voucher.DiscountValue / 100;
                if (voucher.MaxDiscountAmount.HasValue && discountAmount > voucher.MaxDiscountAmount.Value)
                {
                    discountAmount = voucher.MaxDiscountAmount.Value;
                }
            }
            else
            {
                discountAmount = voucher.DiscountValue;
            }

            return Ok(new
            {
                valid = true,
                voucherId = voucher.Id,
                code = voucher.Code,
                discountType = voucher.DiscountType,
                discountValue = voucher.DiscountValue,
                discountAmount = discountAmount,
                message = $"Giảm {discountAmount:N0}₫"
            });
        }
    }

    public class ValidateVoucherRequest
    {
        public string Code { get; set; } = string.Empty;
        public decimal OrderAmount { get; set; }
    }
}
