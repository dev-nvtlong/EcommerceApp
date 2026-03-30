using EcommerceApp.Application.DTOs.Account;

namespace EcommerceApp.Application.DTOs.Blog
{
    public class BlogPostDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string? Content { get; set; }
        public string? Thumbnail { get; set; }
        public int UserId { get; set; }
        public string? AuthorName { get; set; }
        public bool IsPublished { get; set; }
        public DateTime? CreatedAt { get; set; }

        public int LikeCount { get; set; }
        public bool IsLikedByCurrentUser { get; set; }
        public List<CommentDto> Comments { get; set; } = new List<CommentDto>();
    }

    public class CommentDto
    {
        public int Id { get; set; }
        public int BlogPostId { get; set; }
        public int UserId { get; set; }
        public string? UserName { get; set; }
        public string? UserAvatar { get; set; }
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
