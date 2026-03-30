using EcommerceApp.Base;

namespace EcommerceApp.Models
{
    public class Cart : BaseEntity
    {
        public int UserId { get; set; }
        public ApplicationUser User { get; set; }

        public ICollection<CartItem>? Items { get; set; }
    }
}
