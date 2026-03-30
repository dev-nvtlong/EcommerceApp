using EcommerceApp.Enums;

namespace EcommerceApp.Application.DTOs.Order
{
    public class OrderDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public DateTime OrderDate { get; set; }
        private decimal _totalAmount;
        public decimal TotalAmount 
        { 
            get => (_totalAmount == 0 && Details != null && Details.Any()) ? Details.Sum(d => d.SubTotal) : _totalAmount;
            set => _totalAmount = value;
        }
        public OrderStatus Status { get; set; }
        public PaymentMethod PaymentMethod { get; set; }

        public string? CustomerName { get; set; }
        public string? CustomerEmail { get; set; }
        public string? ShipAddress { get; set; }
        public string? ShipPhone { get; set; }
        public string? ShipName { get; set; }

        public ICollection<OrderDetailDto> Details { get; set; } = new List<OrderDetailDto>();
    }
}
