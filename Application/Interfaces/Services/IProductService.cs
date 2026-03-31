using EcommerceApp.Application.DTOs.Product;

namespace EcommerceApp.Application.Interfaces.Services
{
    public interface IProductService
    {
        Task<List<ProductDto>> GetAllAsync();
        Task<ProductDto?> GetByIdAsync(int id);
        Task<ProductDto?> GetByIdAsNoTrackingAsync(int id);
        Task CreateAsync(ProductDto dto);
        Task UpdateAsync(ProductDto dto);
        Task DeleteAsync(int id);
        Task<List<ProductDto>> SearchAsync(string searchTerm);
    }
}
