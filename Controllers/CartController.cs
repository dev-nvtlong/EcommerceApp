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

        public CartController(ICartService cartService)
        {
            _cartService = cartService;
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
            return Json(new { success = true });
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
    }
}
