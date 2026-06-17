using EcommerceApp.Base;
using EcommerceApp.Enums;
using EcommerceApp.Models.Entities;

namespace EcommerceApp.Models
{
    public class Order : BaseEntity
    {
        public Guid UserId { get; set; }
        public User User { get; set; }
        public DateTime OrderDate { get; set; } = DateTime.UtcNow;
        public decimal TotalAmount { get; set; }

        public OrderStatus Status { get; set; } = OrderStatus.Pending;
        public PaymentMethod PaymentMethod { get; set; }

        public string? ShipName { get; set; }
        public string? ShipAddress { get; set; }
        public string? ShipPhone { get; set; }

        public ICollection<OrderDetail>? Details { get; set; } = new List<OrderDetail>();
    }

}
