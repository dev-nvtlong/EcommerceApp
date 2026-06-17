using EcommerceApp.Base;
using EcommerceApp.Enums;
using EcommerceApp.Models.Entities;

namespace EcommerceApp.Models
{
    public class BlogPost : BaseEntity
    {
        public string Title { get; set; } = null!;
        public string? Content { get; set; }

        public string? Thumbnail { get; set; }

        public BlogCategory Category { get; set; } = BlogCategory.Experience;
        public string? Tags { get; set; }

        public Guid UserId { get; set; }
        public User User { get; set; }

        public bool IsPublished { get; set; }

        public ICollection<Comment>? Comments { get; set; }
        public ICollection<Like>? Likes { get; set; }
        public ICollection<BlogPostImage>? Images { get; set; }
    }
}
