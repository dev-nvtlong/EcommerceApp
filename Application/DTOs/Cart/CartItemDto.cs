using EcommerceApp.Application.DTOs.Product;

namespace EcommerceApp.Application.DTOs.Cart
{
    public class CartItemDto
    {
        public Guid Id { get; set; }
        public Guid CartId { get; set; }
        public Guid ProductId { get; set; }
        public ProductDto? Product { get; set; }
        public int Quantity { get; set; }
        
        public decimal SubTotal => (Product?.Price ?? 0) * Quantity;
    }
}
