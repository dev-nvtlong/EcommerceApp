using EcommerceApp.Application.DTOs.Order;

namespace EcommerceApp.Application.Interfaces.Services
{
    public interface IOrderService
    {
        Task<OrderDto> CreateOrderAsync(int userId, CreateOrderDto createOrderDto);
        Task<List<OrderDto>> GetOrdersByUserIdAsync(int userId);
        Task<OrderDto?> GetOrderByIdAsync(int orderId);
        Task<List<OrderDto>> GetAllOrdersAsync();
        Task UpdateOrderStatusAsync(int orderId, EcommerceApp.Enums.OrderStatus status);
        Task UpdateOrderAsync(int orderId, OrderDto orderDto);
    }
}
