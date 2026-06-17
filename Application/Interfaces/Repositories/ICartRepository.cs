using EcommerceApp.Models;

namespace EcommerceApp.Application.Interfaces.Repositories
{
    public interface ICartRepository
    {
        Task<Cart?> GetByUserIdAsync(Guid userId);
        Task<CartItem?> GetCartItemAsync(Guid cartId, Guid productId);
        Task CreateCartAsync(Cart cart);
        Task AddCartItemAsync(CartItem item);
        Task UpdateCartItemAsync(CartItem item);
        Task RemoveCartItemAsync(CartItem item);
        Task SaveAsync();
    }
}
