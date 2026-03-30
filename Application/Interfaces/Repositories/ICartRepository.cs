using EcommerceApp.Models;

namespace EcommerceApp.Application.Interfaces.Repositories
{
    public interface ICartRepository
    {
        Task<Cart?> GetByUserIdAsync(int userId);
        Task<CartItem?> GetCartItemAsync(int cartId, int productId);
        Task CreateCartAsync(Cart cart);
        Task AddCartItemAsync(CartItem item);
        Task UpdateCartItemAsync(CartItem item);
        Task RemoveCartItemAsync(CartItem item);
        Task SaveAsync();
    }
}
