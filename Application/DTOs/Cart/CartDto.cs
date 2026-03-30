namespace EcommerceApp.Application.DTOs.Cart
{
    public class CartDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public List<CartItemDto> Items { get; set; } = new();
        
        public decimal TotalAmount => Items.Sum(i => i.SubTotal);
        public int TotalItems => Items.Sum(i => i.Quantity);
    }
}
