using EcommerceApp.Application.DTOs.Account;
using EcommerceApp.Enums;

namespace EcommerceApp.Application.DTOs.Blog
{
    public class BlogPostDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string? Content { get; set; }
        public string? Thumbnail { get; set; }

        public BlogCategory Category { get; set; }
        public string? Tags { get; set; }

        public Guid UserId { get; set; }
        public string? AuthorName { get; set; }
        public bool IsPublished { get; set; }
        public DateTime? CreatedAt { get; set; }

        public int LikeCount { get; set; }
        public bool IsLikedByCurrentUser { get; set; }
        public List<string>? ImageUrls { get; set; }
        public List<CommentDto> Comments { get; set; } = new List<CommentDto>();
    }

    public class CommentDto
    {
        public Guid Id { get; set; }
        public Guid BlogPostId { get; set; }
        public Guid UserId { get; set; }
        public string? UserName { get; set; }
        public string? UserAvatar { get; set; }
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
