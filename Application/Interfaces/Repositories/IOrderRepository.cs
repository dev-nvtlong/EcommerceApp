using EcommerceApp.Models;

namespace EcommerceApp.Application.Interfaces.Repositories
{
    public interface IOrderRepository
    {
        Task<Order> CreateAsync(Order order);
        Task UpdateAsync(Order order);
        Task DeleteAsync(Guid id);
        Task<Order?> GetByIdAsync(Guid id);
        Task<List<Order>> GetAllByUserIdAsync(Guid userId);
        Task<List<Order>> GetAllWithUserAsync();
    }
}
