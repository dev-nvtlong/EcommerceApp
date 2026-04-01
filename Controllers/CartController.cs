using System.Security.Claims;
using EcommerceApp.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EcommerceApp.Controllers
{
    [Authorize]
    public class CartController : Controller
    {
        private readonly ICartService _cartService;
        private readonly IProductService _productService;

        public CartController(ICartService cartService, IProductService productService)
        {
            _cartService = cartService;
            _productService = productService;
        }

        private int UserId => int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        public async Task<IActionResult> Index()
        {
            var cart = await _cartService.GetCartByUserIdAsync(UserId);
            return View(cart);
        }

        [HttpPost]
        public async Task<IActionResult> AddToCart(int productId, int quantity = 1)
        {
            await _cartService.AddToCartAsync(UserId, productId, quantity);
            var product = await _productService.GetByIdAsync(productId);
            
            return Json(new { 
                success = true, 
                product = new { 
                    name = product?.Name, 
                    imageUrl = product?.ImageUrls?.FirstOrDefault() 
                } 
            });
        }

        [HttpPost]
        public async Task<IActionResult> UpdateQuantity(int productId, int quantity)
        {
            await _cartService.UpdateQuantityAsync(UserId, productId, quantity);
            return Json(new { success = true });
        }

        [HttpPost]
        public async Task<IActionResult> RemoveItem(int productId)
        {
            await _cartService.RemoveFromCartAsync(UserId, productId);
            return Json(new { success = true });
        }

        [HttpGet]
        public async Task<IActionResult> GetCartSummary()
        {
            var cart = await _cartService.GetCartByUserIdAsync(UserId);
            return Json(new { totalItems = cart.TotalItems });
        }

        [HttpGet]
        public async Task<IActionResult> GetCartSummaryFull()
        {
            var cart = await _cartService.GetCartByUserIdAsync(UserId);
            var items = cart.Items.Select(i => new {
                productId = i.ProductId,
                name = i.Product.Name,
                imageUrl = i.Product.ImageUrls?.FirstOrDefault(),
                price = i.Product.Price,
                quantity = i.Quantity
            });

            return Json(new { 
                totalItems = cart.TotalItems, 
                totalAmount = cart.TotalAmount,
                items = items
            });
        }
    }
}
