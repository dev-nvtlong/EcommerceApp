using EcommerceApp.Application.DTOs.Order;

namespace EcommerceApp.Application.Interfaces.Services
{
    public interface IOrderService
    {
        Task<OrderDto> CreateOrderAsync(Guid userId, CreateOrderDto createOrderDto);
        Task<List<OrderDto>> GetOrdersByUserIdAsync(Guid userId);
        Task<OrderDto?> GetOrderByIdAsync(Guid orderId);
        Task<List<OrderDto>> GetAllOrdersAsync();
        Task UpdateOrderStatusAsync(Guid orderId, EcommerceApp.Enums.OrderStatus status);
        Task UpdateOrderAsync(Guid orderId, OrderDto orderDto);
    }
}
