using AutoMapper;
using Core.Application.DTOs;
using Core.Domain.Entities;
using System.Linq;

namespace Core.Application.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Product Mappings
            CreateMap<Product, ProductDto>()
                .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category != null ? src.Category.Name : "Uncategorized"));
            
            CreateMap<CreateProductDto, Product>();

            CreateMap<ProductOptionGroup, ProductOptionGroupDto>();
            CreateMap<ProductOptionValue, ProductOptionValueDto>();

            // Order Mappings
            CreateMap<Order, OrderDto>();
            CreateMap<OrderDetail, OrderDetailDto>();
            CreateMap<OrderDetailOption, OrderDetailOptionDto>();
        }
    }
}
