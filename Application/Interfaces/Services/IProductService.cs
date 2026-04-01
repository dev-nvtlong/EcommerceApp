using EcommerceApp.Application.DTOs.Product;

namespace EcommerceApp.Application.Interfaces.Services
{
    public interface IProductService
    {
        Task<List<ProductDto>> GetAllAsync();
        Task<ProductDto?> GetByIdAsync(int id);
        Task<ProductDto?> GetByIdAsNoTrackingAsync(int id);
        Task<int> CreateAsync(ProductDto dto);
        Task UpdateAsync(ProductDto dto);
        Task DeleteAsync(int id);
        Task<List<ProductDto>> GetAllActiveAsync();
        Task<List<ProductDto>> SearchAsync(string searchTerm);
        Task ImportStockAsync(int productId, int quantity, decimal costPrice);
    }
}
