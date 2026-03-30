namespace EcommerceApp.Application.DTOs.Social
{
    public class CommentDto
    {
        public int Id { get; set; }
        public int BlogPostId { get; set; }
        public string BlogPostTitle { get; set; }

        public int UserId { get; set; }
        public string UserFullName { get; set; }
        public string UserAvatar { get; set; }

        public string Content { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    public class LikeDto
    {
        public int UserId { get; set; }
        public string UserFullName { get; set; }
        public string UserAvatar { get; set; }

        public int BlogPostId { get; set; }
        public string BlogPostTitle { get; set; }
    }
}
