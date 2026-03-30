using EcommerceApp.Application.DTOs.Product;
using EcommerceApp.Application.DTOs.Account;

namespace EcommerceApp.Application.DTOs.Review
{
    public class ReviewDto
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string ProductThumbnail { get; set; }

        public int UserId { get; set; }
        public string UserFullName { get; set; }
        public string UserAvatar { get; set; }

        public int Rating { get; set; }
        public string Comment { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
