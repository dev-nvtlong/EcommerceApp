using EcommerceApp.Enums;

namespace EcommerceApp.Application.DTOs.Order
{
    public class CreateOrderDto
    {
        public int UserId { get; set; }
        public PaymentMethod PaymentMethod { get; set; }
    }
}
