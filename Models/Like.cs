using EcommerceApp.Models.Entities;

namespace EcommerceApp.Models
{
    public class Like
    {
        public Guid UserId { get; set; }
        public User User { get; set; }

        public Guid BlogPostId { get; set; }
        public BlogPost BlogPost { get; set; }
    }

}
