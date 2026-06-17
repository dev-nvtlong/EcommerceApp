namespace EcommerceApp.Application.DTOs.Social
{
    public class CommentDto
    {
        public Guid Id { get; set; }
        public Guid BlogPostId { get; set; }
        public string BlogPostTitle { get; set; }

        public Guid UserId { get; set; }
        public string UserFullName { get; set; }
        public string UserAvatar { get; set; }

        public string Content { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    public class LikeDto
    {
        public Guid UserId { get; set; }
        public string UserFullName { get; set; }
        public string UserAvatar { get; set; }

        public Guid BlogPostId { get; set; }
        public string BlogPostTitle { get; set; }
    }
}
