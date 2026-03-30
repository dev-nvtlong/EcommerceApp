using EcommerceApp.Models;

namespace EcommerceApp.Application.Interfaces.Repositories
{
    public interface IOrderRepository
    {
        Task<Order> CreateAsync(Order order);
        Task UpdateAsync(Order order);
        Task DeleteAsync(int id);
        Task<Order?> GetByIdAsync(int id);
        Task<List<Order>> GetAllByUserIdAsync(int userId);
        Task<List<Order>> GetAllWithUserAsync();
    }
}
