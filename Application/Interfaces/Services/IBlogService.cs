using EcommerceApp.Application.DTOs.Blog;
using EcommerceApp.Enums;

namespace EcommerceApp.Application.Interfaces.Services
{
    public interface IBlogService
    {
        // Client side
        Task<List<BlogPostDto>> GetPublishedPostsAsync(string? searchTerm = null, BlogCategory? category = null);
        Task<BlogPostDto?> GetPostDetailsAsync(Guid id, Guid? currentUserId = null);
        Task<bool> ToggleLikeAsync(Guid postId, Guid userId);
        Task<CommentDto> AddCommentAsync(Guid postId, Guid userId, string content);

        // Admin side
        Task<List<BlogPostDto>> GetAllPostsAsync();
        Task<BlogPostDto> CreatePostAsync(Guid userId, CreateBlogPostDto dto);
        Task<bool> UpdatePostAsync(Guid id, CreateBlogPostDto dto);
        Task<bool> DeletePostAsync(Guid id);
        Task<bool> AddPostImagesAsync(Guid postId, List<string> imageUrls);
    }
}
