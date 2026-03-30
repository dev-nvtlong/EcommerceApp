using AutoMapper;
using EcommerceApp.Application.DTOs.Social;
using EcommerceApp.Models;

namespace EcommerceApp.Mappings
{
    public class SocialProfile : Profile
    {
        public SocialProfile()
        {
            CreateMap<Comment, CommentDto>()
                .ForMember(dest => dest.BlogPostTitle, opt => opt.MapFrom(src => src.BlogPost.Title))
                .ForMember(dest => dest.UserFullName, opt => opt.MapFrom(src => src.User.FullName ?? src.User.UserName))
                .ForMember(dest => dest.UserAvatar, opt => opt.MapFrom(src => src.User.Avatar));

            CreateMap<Like, LikeDto>()
                .ForMember(dest => dest.BlogPostTitle, opt => opt.MapFrom(src => src.BlogPost.Title))
                .ForMember(dest => dest.UserFullName, opt => opt.MapFrom(src => src.User.FullName ?? src.User.UserName))
                .ForMember(dest => dest.UserAvatar, opt => opt.MapFrom(src => src.User.Avatar));
        }
    }
}
