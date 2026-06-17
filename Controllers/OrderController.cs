using EcommerceApp.Application.DTOs.Order;
using EcommerceApp.Application.Interfaces.Services;
using EcommerceApp.Models;
using EcommerceApp.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace EcommerceApp.Controllers
{
    [Authorize]
    public class OrderController : Controller
    {
        private readonly IOrderService _orderService;
        private readonly ICartService _cartService;
        private readonly ApplicationDbContext _context;

        public OrderController(IOrderService orderService, ICartService cartService, ApplicationDbContext context)
        {
            _orderService = orderService;
            _cartService = cartService;
            _context = context;
        }

        private Guid UserId => Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        public async Task<IActionResult> Index(string? status = null)
        {
            var orders = await _orderService.GetOrdersByUserIdAsync(UserId);
            
            ViewBag.ActiveStatus = status?.ToLower();
            
            if (!string.IsNullOrEmpty(status))
            {
                switch (status.ToLower())
                {
                    case "pending":
                        orders = orders.Where(o => o.Status == EcommerceApp.Enums.OrderStatus.Pending || 
                                                 o.Status == EcommerceApp.Enums.OrderStatus.Confirmed).ToList();
                        break;
                    case "shipping":
                        orders = orders.Where(o => o.Status == EcommerceApp.Enums.OrderStatus.Shipping).ToList();
                        break;
                    case "completed":
                        orders = orders.Where(o => o.Status == EcommerceApp.Enums.OrderStatus.Completed).ToList();
                        break;
                    case "cancelled":
                        orders = orders.Where(o => o.Status == EcommerceApp.Enums.OrderStatus.Cancelled).ToList();
                        break;
                }
            }
            
            return View(orders);
        }

        public async Task<IActionResult> Details(Guid id)
        {
            var order = await _orderService.GetOrderByIdAsync(id);
            if (order == null || order.UserId != UserId)
            {
                return NotFound();
            }
            return View(order);
        }

        [HttpGet]
        public async Task<IActionResult> Checkout()
        {
            var cart = await _cartService.GetCartByUserIdAsync(UserId);
            if (cart == null || cart.Items == null || !cart.Items.Any())
            {
                return RedirectToAction("Index", "Cart");
            }

            // Load temporary shipping info from session if set
            ViewBag.TempAddress = HttpContext.Session.GetString("TempShipAddress");
            ViewBag.TempPhone = HttpContext.Session.GetString("TempShipPhone");

            // Load official profile info for fallback/display
            var user = await _context.Users.FindAsync(UserId);
            if (user != null)
            {
                ViewBag.ProfileAddress = "Cần cập nhật địa chỉ";
            }

            return View(cart);
        }

        [HttpPost]
        public async Task<IActionResult> PlaceOrder(CreateOrderDto model)
        {
            try
            {
                // Override with temporary shipping info if set on product page
                var tempAddress = HttpContext.Session.GetString("TempShipAddress");
                var tempPhone = HttpContext.Session.GetString("TempShipPhone");

                if (!string.IsNullOrEmpty(tempAddress)) model.ShipAddress = tempAddress;
                if (!string.IsNullOrEmpty(tempPhone)) model.ShipPhone = tempPhone;

                var order = await _orderService.CreateOrderAsync(UserId, model);

                // Clear session after order placed
                HttpContext.Session.Remove("TempShipAddress");
                HttpContext.Session.Remove("TempShipPhone");

                return Json(new { success = true, orderId = order.Id });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> CancelOrder(Guid id, string reason)
        {
            try
            {
                var order = await _orderService.GetOrderByIdAsync(id);
                if (order == null || order.UserId != UserId)
                    return Json(new { success = false, message = "Không tìm thấy đơn hàng." });

                if (order.Status != EcommerceApp.Enums.OrderStatus.Pending && order.Status != EcommerceApp.Enums.OrderStatus.Confirmed)
                    return Json(new { success = false, message = "Đơn hàng này không thể hủy." });

                if (order.PaymentMethod == EcommerceApp.Enums.PaymentMethod.Banking)
                    return Json(new { success = false, message = "Đơn hàng chuyển khoản cần liên hệ Zalo để hủy." });

                await _orderService.UpdateOrderStatusAsync(id, EcommerceApp.Enums.OrderStatus.Cancelled);
                
                TempData["Success"] = "Đã hủy đơn hàng thành công.";
                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        public IActionResult Success(Guid id)
        {
            ViewBag.OrderId = id;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Reorder(Guid id)
        {
            try
            {
                var order = await _orderService.GetOrderByIdAsync(id);
                if (order == null || order.UserId != UserId)
                {
                    return Json(new { success = false, message = "Không tìm thấy đơn hàng." });
                }

                // Add items from the old order to the cart
                foreach (var item in order.Details)
                {
                    await _cartService.AddToCartAsync(UserId, item.ProductId, item.Quantity);
                }

                return Json(new { success = true, redirectUrl = Url.Action("Index", "Cart") });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }
    }
}
