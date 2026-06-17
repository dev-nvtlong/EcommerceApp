using EcommerceApp.Application.DTOs.Cart;

namespace EcommerceApp.Application.Interfaces.Services
{
    public interface ICartService
    {
        Task<CartDto> GetCartByUserIdAsync(Guid userId);
        Task AddToCartAsync(Guid userId, Guid productId, int quantity);
        Task UpdateQuantityAsync(Guid userId, Guid productId, int quantity);
        Task RemoveFromCartAsync(Guid userId, Guid productId);
        Task ClearCartAsync(Guid userId);
    }
}
