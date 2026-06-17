using EcommerceApp.Base;
using EcommerceApp.Models.Entities;

namespace EcommerceApp.Models
{
    public class Review : BaseEntity
    {
        public Guid ProductId { get; set; }
        public Product Product { get; set; }

        public Guid UserId { get; set; }
        public User User { get; set; }

        public int Rating { get; set; }
        public string? Comment { get; set; }

        public DateTime CreatedAt { get; set; }
    }

}
