using EcommerceApp.Base;
using EcommerceApp.Enums;

namespace EcommerceApp.Models
{
    public class Payment : BaseEntity
    {
        public int OrderId { get; set; }
        public Order Order { get; set; }

        public decimal Amount { get; set; }

        public PaymentMethod Method { get; set; }
        public PaymentStatus Status { get; set; } = PaymentStatus.Pending;

        public DateTime PaidAt { get; set; }
    }

}
