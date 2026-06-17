using EcommerceApp.Base;

namespace EcommerceApp.Models
{
    public class Post : BaseEntity
    { 
        public string Title { get; set; }
        public string Content { get; set; }
        public string? ImageUrl { get; set; }
        public string AuthorId { get; set; }

    }
}
