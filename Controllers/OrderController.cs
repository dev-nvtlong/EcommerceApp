using System.Security.Claims;
using EcommerceApp.Application.DTOs.Order;
using EcommerceApp.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EcommerceApp.Controllers
{
    [Authorize]
    public class OrderController : Controller
    {
        private readonly IOrderService _orderService;
        private readonly ICartService _cartService;

        public OrderController(IOrderService orderService, ICartService cartService)
        {
            _orderService = orderService;
            _cartService = cartService;
        }

        private int UserId => int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        public async Task<IActionResult> Index()
        {
            var orders = await _orderService.GetOrdersByUserIdAsync(UserId);
            return View(orders);
        }

        public async Task<IActionResult> Details(int id)
        {
            var order = await _orderService.GetOrderByIdAsync(id);
            if (order == null || order.UserId != UserId)
            {
                return NotFound();
            }
            return View(order);
        }

        public async Task<IActionResult> Checkout()
        {
            var cart = await _cartService.GetCartByUserIdAsync(UserId);
            if (cart == null || cart.Items == null || !cart.Items.Any())
            {
                return RedirectToAction("Index", "Cart");
            }
            return View(cart);
        }

        [HttpPost]
        public async Task<IActionResult> PlaceOrder(CreateOrderDto model)
        {
            try
            {
                var order = await _orderService.CreateOrderAsync(UserId, model);
                return Json(new { success = true, orderId = order.Id });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        public IActionResult Success(int id)
        {
            ViewBag.OrderId = id;
            return View();
        }
    }
}
