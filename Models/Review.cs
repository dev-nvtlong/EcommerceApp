using EcommerceApp.Base;

namespace EcommerceApp.Models
{
    public class Review : BaseEntity
    {
        public int ProductId { get; set; }
        public Product Product { get; set; }

        public int UserId { get; set; }
        public ApplicationUser User { get; set; }

        public int Rating { get; set; }
        public string? Comment { get; set; }

        public DateTime CreatedAt { get; set; }
    }

}
