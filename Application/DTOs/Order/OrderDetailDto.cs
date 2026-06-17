using EcommerceApp.Application.DTOs.Product;

namespace EcommerceApp.Application.DTOs.Order
{
    public class OrderDetailDto
    {
        public Guid Id { get; set; }
        public Guid OrderId { get; set; }
        public Guid ProductId { get; set; }
        public ProductDto? Product { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public decimal SubTotal => Price * Quantity;
    }
}
