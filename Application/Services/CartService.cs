using AutoMapper;
using EcommerceApp.Application.DTOs.Cart;
using EcommerceApp.Application.Interfaces.Repositories;
using EcommerceApp.Application.Interfaces.Services;
using EcommerceApp.Models;

namespace EcommerceApp.Application.Services
{
    public class CartService : ICartService
    {
        private readonly ICartRepository _cartRepository;
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;

        public CartService(ICartRepository cartRepository, IProductRepository productRepository, IMapper mapper)
        {
            _cartRepository = cartRepository;
            _productRepository = productRepository;
            _mapper = mapper;
        }

        public async Task<CartDto> GetCartByUserIdAsync(int userId)
        {
            var cart = await _cartRepository.GetByUserIdAsync(userId);
            if (cart == null)
            {
                cart = new Cart { UserId = userId };
                await _cartRepository.CreateCartAsync(cart);
                await _cartRepository.SaveAsync();
            }
            return _mapper.Map<CartDto>(cart);
        }

        public async Task AddToCartAsync(int userId, int productId, int quantity)
        {
            var cart = await _cartRepository.GetByUserIdAsync(userId);
            if (cart == null)
            {
                cart = new Cart { UserId = userId };
                await _cartRepository.CreateCartAsync(cart);
                await _cartRepository.SaveAsync();
            }

            var cartItem = await _cartRepository.GetCartItemAsync(cart.ID, productId);
            if (cartItem != null)
            {
                cartItem.Quantity += quantity;
                await _cartRepository.UpdateCartItemAsync(cartItem);
            }
            else
            {
                cartItem = new CartItem
                {
                    CartId = cart.ID,
                    ProductId = productId,
                    Quantity = quantity
                };
                await _cartRepository.AddCartItemAsync(cartItem);
            }
            await _cartRepository.SaveAsync();
        }

        public async Task UpdateQuantityAsync(int userId, int productId, int quantity)
        {
            var cart = await _cartRepository.GetByUserIdAsync(userId);
            if (cart == null) return;

            var cartItem = await _cartRepository.GetCartItemAsync(cart.ID, productId);
            if (cartItem != null)
            {
                if (quantity > 0)
                {
                    cartItem.Quantity = quantity;
                    await _cartRepository.UpdateCartItemAsync(cartItem);
                }
                else
                {
                    await _cartRepository.RemoveCartItemAsync(cartItem);
                }
                await _cartRepository.SaveAsync();
            }
        }

        public async Task RemoveFromCartAsync(int userId, int productId)
        {
            var cart = await _cartRepository.GetByUserIdAsync(userId);
            if (cart == null) return;

            var cartItem = await _cartRepository.GetCartItemAsync(cart.ID, productId);
            if (cartItem != null)
            {
                await _cartRepository.RemoveCartItemAsync(cartItem);
                await _cartRepository.SaveAsync();
            }
        }

        public async Task ClearCartAsync(int userId)
        {
            var cart = await _cartRepository.GetByUserIdAsync(userId);
            if (cart != null && cart.Items != null)
            {
                foreach (var item in cart.Items.ToList())
                {
                    await _cartRepository.RemoveCartItemAsync(item);
                }
                await _cartRepository.SaveAsync();
            }
        }
    }
}
