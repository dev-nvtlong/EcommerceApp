using AutoMapper;
using EcommerceApp.Application.DTOs.Product;
using EcommerceApp.Models;

namespace EcommerceApp.Mappings
{
    public class ProductProfile : Profile
    {
        public ProductProfile()
        {
            CreateMap<Product, ProductDto>()
                .ForMember(dest => dest.ProductId, opt => opt.MapFrom(src => src.ID))
                .ForMember(dest => dest.ImageUrls,
                    opt => opt.MapFrom(src => src.Images.Select(i => i.ImageUrl)));

            CreateMap<ProductDto, Product>()
                .ForMember(dest => dest.ID, opt => opt.MapFrom(src => src.ProductId))
                .ForMember(dest => dest.Images,
                    opt => opt.MapFrom(src => src.ImageUrls != null 
                        ? src.ImageUrls.Select(url => new ProductImage { ImageUrl = url, IsMain = true }).ToList() 
                        : new List<ProductImage>()));
        }
    }
}
