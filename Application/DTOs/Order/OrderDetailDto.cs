using EcommerceApp.Application.DTOs.Product;

namespace EcommerceApp.Application.DTOs.Order
{
    public class OrderDetailDto
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public ProductDto? Product { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public decimal SubTotal => Price * Quantity;
    }
}
