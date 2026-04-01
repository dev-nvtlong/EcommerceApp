using EcommerceApp.Base;

namespace EcommerceApp.Models
{
    public class BlogPostImage : BaseEntity
    {
        public int BlogPostId { get; set; }
        public BlogPost BlogPost { get; set; }

        public string ImageUrl { get; set; }
        public int SortOrder { get; set; }
    }
}
