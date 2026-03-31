using EcommerceApp.Base;
using EcommerceApp.Enums;

namespace EcommerceApp.Models
{
    public class BlogPost : AuditableEntity
    {
        public string Title { get; set; }
        public string? Content { get; set; }

        public string? Thumbnail { get; set; }

        public BlogCategory Category { get; set; } = BlogCategory.Experience;
        public string? Tags { get; set; }

        public int UserId { get; set; }
        public ApplicationUser User { get; set; }

        public bool IsPublished { get; set; }

        public ICollection<Comment>? Comments { get; set; }
        public ICollection<Like>? Likes { get; set; }
        public ICollection<BlogPostImage>? Images { get; set; }
    }
}
