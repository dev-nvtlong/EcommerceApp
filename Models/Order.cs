using EcommerceApp.Base;
using EcommerceApp.Enums;

namespace EcommerceApp.Models
{
    public class Order : AuditableEntity
    {
        public int UserId { get; set; }
        public ApplicationUser User { get; set; }

        public decimal TotalAmount { get; set; }

        public OrderStatus Status { get; set; } = OrderStatus.Pending;
        public PaymentMethod PaymentMethod { get; set; }

        public ICollection<OrderDetail>? Details { get; set; }
    }

}
