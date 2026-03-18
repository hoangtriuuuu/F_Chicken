using Core.Application.Interfaces;
using Core.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers.Admin
{
    [ApiController]
    [Route("api/admin/[controller]")]
    [Authorize(Policy = "Admin")]
    public class CategoriesController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public CategoriesController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var categories = await _unitOfWork.Categories.GetAllAsync();
            return Ok(categories);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var category = await _unitOfWork.Categories.GetByIdAsync(id);
            if (category == null)
                return NotFound(new { message = "Danh mục không tồn tại" });
            return Ok(category);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CategoryDto dto)
        {
            var category = new Category
            {
                Name = dto.Name,
                Description = dto.Description,
                IsActive = true
            };

            await _unitOfWork.Categories.AddAsync(category);
            await _unitOfWork.CompleteAsync();

            return Ok(new { success = true, message = "Thêm danh mục thành công", data = category });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] CategoryDto dto)
        {
            var category = await _unitOfWork.Categories.GetByIdAsync(id);
            if (category == null)
                return NotFound(new { message = "Danh mục không tồn tại" });

            category.Name = dto.Name;
            category.Description = dto.Description;

            await _unitOfWork.Categories.UpdateAsync(category);
            await _unitOfWork.CompleteAsync();

            return Ok(new { success = true, message = "Cập nhật danh mục thành công", data = category });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var category = await _unitOfWork.Categories.GetByIdAsync(id);
            if (category == null)
                return NotFound(new { message = "Danh mục không tồn tại" });

            await _unitOfWork.Categories.DeleteAsync(category);
            await _unitOfWork.CompleteAsync();

            return Ok(new { success = true, message = "Xóa danh mục thành công" });
        }

        [HttpPut("{id}/toggle-active")]
        public async Task<IActionResult> ToggleActive(int id)
        {
            var category = await _unitOfWork.Categories.GetByIdAsync(id);
            if (category == null)
                return NotFound(new { message = "Danh mục không tồn tại" });

            category.IsActive = !category.IsActive;
            await _unitOfWork.Categories.UpdateAsync(category);
            await _unitOfWork.CompleteAsync();

            return Ok(new { success = true, message = category.IsActive ? "Đã kích hoạt danh mục" : "Đã ẩn danh mục", data = category });
        }
    }

    public class CategoryDto
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
    }
}
