namespace EcommerceApp.Application.DTOs.Product
{
    public class ProductDto
    {
        public int ProductId { get; set; }

        public string Name { get; set; }
        public decimal Price { get; set; }

        public string? Description { get; set; }

        public int CategoryId { get; set; }

        public List<string>? ImageUrls { get; set; }
    }

}
