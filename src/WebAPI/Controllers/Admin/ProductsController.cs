using Core.Application.DTOs;
using Core.Application.Interfaces;
using Core.Application.Services;
using Core.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers.Admin
{
    [ApiController]
    [Route("api/admin/[controller]")]
    [Authorize(Policy = "Admin")]
    public class ProductsController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IProductService _productService;

        public ProductsController(IUnitOfWork unitOfWork, IProductService productService)
        {
            _unitOfWork = unitOfWork;
            _productService = productService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var products = await _unitOfWork.Products.GetAllAsync();
            return Ok(products);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var product = await _unitOfWork.Products.GetByIdAsync(id);
            if (product == null)
                return NotFound(new { message = "Sản phẩm không tồn tại" });

            object response = product;

            if (product.IsCombo)
            {
                var comboItems = await _unitOfWork.ComboDetails.GetAsync(cd => cd.ComboId == id);
                var productDto = new ProductDto
                {
                    Id = product.Id,
                    Name = product.Name,
                    Description = product.Description,
                    BasePrice = product.BasePrice,
                    ImageUrl = product.ImageUrl,
                    IsCombo = product.IsCombo,
                    CategoryId = product.CategoryId,
                    ComboItems = new List<ComboItemDto>()
                };

                foreach (var ci in comboItems)
                {
                    var p = await _unitOfWork.Products.GetByIdAsync(ci.ProductId);
                    productDto.ComboItems.Add(new ComboItemDto
                    {
                        ProductId = ci.ProductId,
                        ProductName = p?.Name ?? "Unknown",
                        ImageUrl = p?.ImageUrl,
                        Quantity = ci.Quantity
                    });
                }
                response = productDto;
            }

            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateProductDto dto)
        {
            try
            {
                var product = new Product
                {
                    Name = dto.Name,
                    Description = dto.Description ?? "",
                    BasePrice = dto.BasePrice,
                    ImageUrl = dto.ImageUrl ?? "https://placehold.co/300x200?text=Product",
                    IsCombo = dto.IsCombo,
                    IsActive = true,
                    CategoryId = dto.CategoryId
                };

                await _unitOfWork.Products.AddAsync(product);
                await _unitOfWork.CompleteAsync(); // Save to get Product Id

                if (dto.IsCombo && dto.ComboItems != null && dto.ComboItems.Any())
                {
                    foreach (var item in dto.ComboItems)
                    {
                        var comboDetail = new ComboDetail
                        {
                            ComboId = product.Id,
                            ProductId = item.ProductId,
                            Quantity = item.Quantity
                        };
                        await _unitOfWork.ComboDetails.AddAsync(comboDetail);
                    }
                    await _unitOfWork.CompleteAsync();
                }

                return Ok(new { success = true, message = "Thêm sản phẩm thành công", id = product.Id });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = ex.Message, inner = ex.InnerException?.Message, stackTrace = ex.StackTrace });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] CreateProductDto dto)
        {
            try
            {
                var product = await _unitOfWork.Products.GetByIdAsync(id);
                if (product == null)
                    return NotFound(new { message = "Sản phẩm không tồn tại" });

                // Kiểm tra và log nếu giá thay đổi
                if (product.BasePrice != dto.BasePrice)
                {
                    int currentUserId = int.TryParse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value, out var uid) ? uid : 1;
                    await _productService.UpdateProductPriceAsync(id, dto.BasePrice, currentUserId);
                }

                product.Name = dto.Name;
                product.Description = dto.Description ?? string.Empty;
                product.ImageUrl = dto.ImageUrl ?? string.Empty;
                product.IsCombo = dto.IsCombo;
                product.CategoryId = dto.CategoryId;

                await _unitOfWork.Products.UpdateAsync(product);

                // Handle Combo Items Update
                if (dto.IsCombo)
                {
                    // Remove existing items
                    var existingItems = await _unitOfWork.ComboDetails.GetAsync(cd => cd.ComboId == id);
                    foreach (var item in existingItems)
                    {
                        await _unitOfWork.ComboDetails.DeleteAsync(item);
                    }

                    // Add new items
                    if (dto.ComboItems != null)
                    {
                        foreach (var item in dto.ComboItems)
                        {
                            var comboDetail = new ComboDetail
                            {
                                ComboId = id,
                                ProductId = item.ProductId,
                                Quantity = item.Quantity
                            };
                            await _unitOfWork.ComboDetails.AddAsync(comboDetail);
                        }
                    }
                }

                await _unitOfWork.CompleteAsync();

                return Ok(new { success = true, message = "Cập nhật sản phẩm thành công", data = product });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new 
                { 
                    success = false, 
                    message = "Lỗi khi cập nhật sản phẩm", 
                    error = ex.Message,
                    innerError = ex.InnerException?.Message,
                    stackTrace = ex.StackTrace
                });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var product = await _unitOfWork.Products.GetByIdAsync(id);
            if (product == null)
                return NotFound(new { message = "Sản phẩm không tồn tại" });

            await _unitOfWork.Products.DeleteAsync(product);
            await _unitOfWork.CompleteAsync();

            return Ok(new { success = true, message = "Xóa sản phẩm thành công" });
        }

        [HttpPut("{id}/toggle-active")]
        public async Task<IActionResult> ToggleActive(int id)
        {
            var product = await _unitOfWork.Products.GetByIdAsync(id);
            if (product == null)
                return NotFound(new { message = "Sản phẩm không tồn tại" });

            product.IsActive = !product.IsActive;
            await _unitOfWork.Products.UpdateAsync(product);
            await _unitOfWork.CompleteAsync();

            return Ok(new { success = true, message = product.IsActive ? "Đã kích hoạt sản phẩm" : "Đã ẩn sản phẩm", data = product });
        }
    }
}
