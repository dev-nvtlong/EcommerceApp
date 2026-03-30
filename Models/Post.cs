using EcommerceApp.Base;

namespace EcommerceApp.Models
{
    public class Post : AuditableEntity
    { 
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string? ImageUrl { get; set; }
        public string AuthorId { get; set; }

    }
}
