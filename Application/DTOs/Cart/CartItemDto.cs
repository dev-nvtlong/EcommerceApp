using EcommerceApp.Application.DTOs.Product;

namespace EcommerceApp.Application.DTOs.Cart
{
    public class CartItemDto
    {
        public int Id { get; set; }
        public int CartId { get; set; }
        public int ProductId { get; set; }
        public ProductDto? Product { get; set; }
        public int Quantity { get; set; }
        
        public decimal SubTotal => (Product?.Price ?? 0) * Quantity;
    }
}
