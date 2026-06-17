using EcommerceApp.Application.DTOs.Product;

namespace EcommerceApp.Application.Interfaces.Services
{
    public interface IProductService
    {
        Task<List<ProductDto>> GetAllAsync();
        Task<ProductDto?> GetByIdAsync(Guid id);
        Task<ProductDto?> GetByIdAsNoTrackingAsync(Guid id);
        Task<Guid> CreateAsync(ProductDto dto);
        Task UpdateAsync(ProductDto dto);
        Task DeleteAsync(Guid id);
        Task<List<ProductDto>> GetAllActiveAsync();
        Task<List<ProductDto>> SearchAsync(string searchTerm);
        Task ImportStockAsync(Guid productId, int quantity, decimal costPrice);
    }
}
