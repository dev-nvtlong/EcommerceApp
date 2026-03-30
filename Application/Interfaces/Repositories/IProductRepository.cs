using EcommerceApp.Models;

namespace EcommerceApp.Application.Interfaces.Repositories
{
    public interface IProductRepository
    {
        Task<List<Product>> GetAllAsync();
        Task<Product?> GetByIdAsync(int id);
        Task<Product?> GetByIdAsNoTrackingAsync(int id);
        Task CreateAsync(Product product);
        Task UpdateAsync(Product product);
        Task DeleteAsync(int id);
        Task SaveAsync();
    }
}
