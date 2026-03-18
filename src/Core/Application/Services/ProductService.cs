using AutoMapper;
using Core.Application.DTOs;
using Core.Application.Interfaces;
using Core.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Core.Application.Services
{

    public class ProductService : IProductService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ProductService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IReadOnlyList<ProductDto>> GetAllProductsAsync()
        {
            // Includes are handled in Repository level or Spec pattern (simplified here)
            // For now assuming the repo returns what we need or we update repo to support Includes later
            // To keep it simple, we will fetch standard list
            var products = await _unitOfWork.Products.GetAllAsync();
            return _mapper.Map<IReadOnlyList<ProductDto>>(products);
        }

        public async Task<ProductDto> GetProductByIdAsync(int id)
        {
            var product = await _unitOfWork.Products.GetByIdAsync(id);
            return _mapper.Map<ProductDto>(product);
        }

        public async Task<ProductDto> CreateProductAsync(CreateProductDto productDto)
        {
            var product = _mapper.Map<Product>(productDto);
            await _unitOfWork.Products.AddAsync(product);
            await _unitOfWork.CompleteAsync();
            return _mapper.Map<ProductDto>(product);
        }

        public async Task<bool> UpdateProductPriceAsync(int productId, decimal newPrice, int processedByUserId)
        {
            var product = await _unitOfWork.Products.GetByIdAsync(productId);
            if (product == null) return false;

            var oldPrice = product.BasePrice;
            if (oldPrice == newPrice) return true;

            var user = await _unitOfWork.Users.GetByIdAsync(processedByUserId);
            var userName = user?.FullName ?? "Unknown";

            product.BasePrice = newPrice;
            product.UpdatedAt = DateTime.UtcNow;

            await _unitOfWork.Products.UpdateAsync(product);

            // Log activity
            try
            {
                await _unitOfWork.AuditLogs.AddAsync(new AuditLog
                {
                    UserId = processedByUserId,
                    UserName = userName,
                    Action = "UpdateProductPrice",
                    EntityName = "Product",
                    EntityId = productId,
                    OldValue = oldPrice.ToString("N0"),
                    NewValue = newPrice.ToString("N0"),
                    CreatedAt = DateTime.UtcNow,
                    Note = $"Thay đổi giá sản phẩm '{product.Name}' từ {oldPrice:N0}đ lên {newPrice:N0}đ"
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR] Không thể ghi AuditLog: {ex.Message}");
                // Vẫn tiếp tục để cập nhật được giá sản phẩm, hoặc throw tùy yêu cầu
                // Ở đây mình chọn throw để đảm bảo Audit Log được thực thi đúng yêu cầu Audit
                throw new Exception($"Lỗi khi ghi lịch sử hệ thống: {ex.Message}", ex);
            }

            return await _unitOfWork.CompleteAsync() > 0;
        }
    }
}
