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

        public double AverageRating { get; set; }
        public int ReviewCount { get; set; }
        public int SoldCount { get; set; }
        public int StockQuantity { get; set; }

        public bool IsActive { get; set; }
        public bool IsFeatured { get; set; }
        public decimal CostPrice { get; set; }
    }

}
