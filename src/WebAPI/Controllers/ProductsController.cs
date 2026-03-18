using System.Collections.Generic;
using System.Linq;
using Core.Application.DTOs;
using Core.Application.Interfaces;
using Core.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("api/product")]
    public class ProductsController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public ProductsController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            // Get all active products
            var products = await _unitOfWork.Products.GetAsync(p => p.IsActive);
            var categories = await _unitOfWork.Categories.GetAllAsync();

            var result = new List<object>();

            foreach (var p in products)
            {
                var productData = new
                {
                    p.Id,
                    p.Name,
                    p.Description,
                    p.BasePrice,
                    p.ImageUrl,
                    p.IsCombo,
                    p.IsActive,
                    p.CategoryId,
                    CategoryName = categories.FirstOrDefault(c => c.Id == p.CategoryId)?.Name ?? "Khác",
                    ComboItems = new List<ComboItemDto>()
                };

                if (p.IsCombo)
                {
                    var comboItems = await _unitOfWork.ComboDetails.GetAsync(cd => cd.ComboId == p.Id);
                    foreach (var ci in comboItems)
                    {
                        var cp = await _unitOfWork.Products.GetByIdAsync(ci.ProductId);
                        productData.ComboItems.Add(new ComboItemDto
                        {
                            ProductId = ci.ProductId,
                            ProductName = cp?.Name ?? "Unknown",
                            ImageUrl = cp?.ImageUrl,
                            Quantity = ci.Quantity
                        });
                    }
                }
                result.Add(productData);
            }

            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var product = await _unitOfWork.Products.GetByIdAsync(id);
            
            if (product == null)
                return NotFound(new { message = "Sản phẩm không tồn tại" });
                
            if (!product.IsActive)
                 return NotFound(new { message = "Sản phẩm không khả dụng" });

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

            if (product.IsCombo)
            {
                var comboItems = await _unitOfWork.ComboDetails.GetAsync(cd => cd.ComboId == id);
                foreach (var ci in comboItems)
                {
                    var cp = await _unitOfWork.Products.GetByIdAsync(ci.ProductId);
                    productDto.ComboItems.Add(new ComboItemDto
                    {
                        ProductId = ci.ProductId,
                        ProductName = cp?.Name ?? "Unknown",
                        ImageUrl = cp?.ImageUrl,
                        Quantity = ci.Quantity
                    });
                }
            }

            return Ok(productDto);
        }
    }
}
