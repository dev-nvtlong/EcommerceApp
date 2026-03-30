using EcommerceApp.Base;

namespace EcommerceApp.Models
{
    public class Comment : BaseEntity
    {
        public int BlogPostId { get; set; }
        public BlogPost BlogPost { get; set; }

        public int UserId { get; set; }
        public ApplicationUser User { get; set; }

        public string Content { get; set; }
        public DateTime CreatedAt { get; set; }
    }

}
