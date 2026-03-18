using Core.Application.Interfaces;
using Core.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("api/admin/vouchers")]
    [Authorize(Roles = "Admin")]
    public class VouchersController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public VouchersController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var vouchers = await _unitOfWork.Vouchers.GetAllAsync();
            return Ok(vouchers);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var voucher = await _unitOfWork.Vouchers.GetByIdAsync(id);
            if (voucher == null) return NotFound();
            return Ok(voucher);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] VoucherDto dto)
        {
            var voucher = new Voucher
            {
                Code = dto.Code.ToUpper(),
                Description = dto.Description,
                DiscountType = dto.DiscountType,
                DiscountValue = dto.DiscountValue,
                MinOrderAmount = dto.MinOrderAmount,
                MaxDiscountAmount = dto.MaxDiscountAmount,
                UsageLimit = dto.UsageLimit,
                StartDate = dto.StartDate,
                EndDate = dto.EndDate,
                IsActive = true
            };

            await _unitOfWork.Vouchers.AddAsync(voucher);
            await _unitOfWork.CompleteAsync();
            return Ok(voucher);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] VoucherDto dto)
        {
            var voucher = await _unitOfWork.Vouchers.GetByIdAsync(id);
            if (voucher == null) return NotFound();

            voucher.Code = dto.Code.ToUpper();
            voucher.Description = dto.Description;
            voucher.DiscountType = dto.DiscountType;
            voucher.DiscountValue = dto.DiscountValue;
            voucher.MinOrderAmount = dto.MinOrderAmount;
            voucher.MaxDiscountAmount = dto.MaxDiscountAmount;
            voucher.UsageLimit = dto.UsageLimit;
            voucher.StartDate = dto.StartDate;
            voucher.EndDate = dto.EndDate;
            voucher.UpdatedAt = DateTime.UtcNow;

            await _unitOfWork.Vouchers.UpdateAsync(voucher);
            await _unitOfWork.CompleteAsync();
            return Ok(voucher);
        }

        [HttpPut("{id}/toggle-active")]
        public async Task<IActionResult> ToggleActive(int id)
        {
            var voucher = await _unitOfWork.Vouchers.GetByIdAsync(id);
            if (voucher == null) return NotFound();

            voucher.IsActive = !voucher.IsActive;
            voucher.UpdatedAt = DateTime.UtcNow;
            await _unitOfWork.Vouchers.UpdateAsync(voucher);
            await _unitOfWork.CompleteAsync();

            return Ok(new { message = voucher.IsActive ? "Đã kích hoạt voucher" : "Đã tắt voucher" });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var voucher = await _unitOfWork.Vouchers.GetByIdAsync(id);
            if (voucher == null) return NotFound();

            await _unitOfWork.Vouchers.DeleteAsync(voucher);
            await _unitOfWork.CompleteAsync();
            return Ok(new { message = "Xóa voucher thành công" });
        }
    }

    public class VoucherDto
    {
        public string Code { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string DiscountType { get; set; } = "Percentage";
        public decimal DiscountValue { get; set; }
        public decimal MinOrderAmount { get; set; }
        public decimal? MaxDiscountAmount { get; set; }
        public int UsageLimit { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}
