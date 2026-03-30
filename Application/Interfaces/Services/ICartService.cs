using EcommerceApp.Application.DTOs.Cart;

namespace EcommerceApp.Application.Interfaces.Services
{
    public interface ICartService
    {
        Task<CartDto> GetCartByUserIdAsync(int userId);
        Task AddToCartAsync(int userId, int productId, int quantity);
        Task UpdateQuantityAsync(int userId, int productId, int quantity);
        Task RemoveFromCartAsync(int userId, int productId);
        Task ClearCartAsync(int userId);
    }
}
