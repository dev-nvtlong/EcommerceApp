using EcommerceApp.Base;
using EcommerceApp.Models.Entities;

namespace EcommerceApp.Models
{
    public class Cart : BaseEntity
    {
        public Guid UserId { get; set; }
        public User User { get; set; }

        public ICollection<CartItem>? Items { get; set; }
    }
}
