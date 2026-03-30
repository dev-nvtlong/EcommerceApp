using EcommerceApp.Application.Interfaces.Repositories;
using EcommerceApp.Data;
using EcommerceApp.Models;
using Microsoft.EntityFrameworkCore;

namespace EcommerceApp.Repositories
{
    public class CartRepository : ICartRepository
    {
        private readonly ApplicationDbContext _context;

        public CartRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Cart?> GetByUserIdAsync(int userId)
        {
            return await _context.Carts
                .Include(c => c.Items!)
                .ThenInclude(i => i.Product)
                .ThenInclude(p => p.Images)
                .FirstOrDefaultAsync(c => c.UserId == userId);
        }

        public async Task<CartItem?> GetCartItemAsync(int cartId, int productId)
        {
            return await _context.CartItems
                .FirstOrDefaultAsync(ci => ci.CartId == cartId && ci.ProductId == productId);
        }

        public async Task CreateCartAsync(Cart cart)
        {
            await _context.Carts.AddAsync(cart);
        }

        public async Task AddCartItemAsync(CartItem item)
        {
            await _context.CartItems.AddAsync(item);
        }

        public async Task UpdateCartItemAsync(CartItem item)
        {
            _context.CartItems.Update(item);
            await Task.CompletedTask;
        }

        public async Task RemoveCartItemAsync(CartItem item)
        {
            _context.CartItems.Remove(item);
            await Task.CompletedTask;
        }

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
