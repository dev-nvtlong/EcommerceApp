namespace EcommerceApp.Models
{
    public class Like
    {
        public int UserId { get; set; }
        public ApplicationUser User { get; set; }

        public int BlogPostId { get; set; }
        public BlogPost BlogPost { get; set; }
    }

}
