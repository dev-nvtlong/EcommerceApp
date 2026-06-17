using EcommerceApp.Base;
using EcommerceApp.Models.Entities;

namespace EcommerceApp.Models
{
    public class Comment : BaseEntity
    {
        public Guid BlogPostId { get; set; }
        public BlogPost BlogPost { get; set; }

        public Guid UserId { get; set; }
        public User User { get; set; }

        public string Content { get; set; }
        public DateTime CreatedAt { get; set; }
    }

}
