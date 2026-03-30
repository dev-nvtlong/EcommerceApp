using AutoMapper;
using EcommerceApp.Application.DTOs.Blog;
using EcommerceApp.Models;

namespace EcommerceApp.Mappings
{
    public class BlogProfile : Profile
    {
        public BlogProfile()
        {
            CreateMap<BlogPost, BlogPostDto>()
                .ForMember(dest => dest.AuthorName, opt => opt.MapFrom(src => src.User.FullName ?? src.User.UserName))
                .ForMember(dest => dest.LikeCount, opt => opt.MapFrom(src => src.Likes != null ? src.Likes.Count : 0))
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.CreatedAt));

            CreateMap<Comment, CommentDto>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.User.FullName ?? src.User.UserName))
                .ForMember(dest => dest.UserAvatar, opt => opt.MapFrom(src => src.User.Avatar));

            CreateMap<CreateBlogPostDto, BlogPost>();
        }
    }
}
