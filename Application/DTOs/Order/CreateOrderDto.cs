using EcommerceApp.Enums;

namespace EcommerceApp.Application.DTOs.Order
{
    public class CreateOrderDto
    {
        public Guid UserId { get; set; }
        public PaymentMethod PaymentMethod { get; set; }
        
        public string? ShipName { get; set; }
        public string? ShipAddress { get; set; }
        public string? ShipPhone { get; set; }
    }
}
