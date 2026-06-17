using AutoMapper;
using EcommerceApp.Application.DTOs.Review;
using EcommerceApp.Models;

namespace EcommerceApp.Mappings
{
    public class ReviewProfile : Profile
    {
        public ReviewProfile()
        {
            CreateMap<Review, ReviewDto>()
                .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.Product.Name))
                .ForMember(dest => dest.ProductThumbnail, opt => opt.MapFrom(src =>
                    src.Product.Images != null && src.Product.Images.Any()
                    ? (src.Product.Images.FirstOrDefault(i => i.IsMain) ?? src.Product.Images.First()).ImageUrl
                    : "/images/no-image.png"))
                .ForMember(dest => dest.UserFullName, opt => opt.MapFrom(src => src.User.Profile != null && !string.IsNullOrEmpty(src.User.Profile.FullName) ? src.User.Profile.FullName : (src.User.UserName ?? "Khách")))
                .ForMember(dest => dest.UserAvatar, opt => opt.MapFrom(src => src.User.Profile != null ? src.User.Profile.AvatarUrl : null));
        }
    }
}
