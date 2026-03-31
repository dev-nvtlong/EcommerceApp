using EcommerceApp.Application.DTOs.Blog;
using EcommerceApp.Enums;

namespace EcommerceApp.Application.Interfaces.Services
{
    public interface IBlogService
    {
        // Client side
        Task<List<BlogPostDto>> GetPublishedPostsAsync(string? searchTerm = null, BlogCategory? category = null);
        Task<BlogPostDto?> GetPostDetailsAsync(int id, int? currentUserId = null);
        Task<bool> ToggleLikeAsync(int postId, int userId);
        Task<CommentDto> AddCommentAsync(int postId, int userId, string content);

        // Admin side
        Task<List<BlogPostDto>> GetAllPostsAsync();
        Task<BlogPostDto> CreatePostAsync(int userId, CreateBlogPostDto dto);
        Task<bool> UpdatePostAsync(int id, CreateBlogPostDto dto);
        Task<bool> DeletePostAsync(int id);
        Task<bool> AddPostImagesAsync(int postId, List<string> imageUrls);
    }
}
