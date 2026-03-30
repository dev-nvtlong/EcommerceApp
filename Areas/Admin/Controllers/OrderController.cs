using EcommerceApp.Application.Interfaces.Services;
using EcommerceApp.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EcommerceApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class OrderController : Controller
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        public async Task<IActionResult> Index()
        {
            var orders = await _orderService.GetAllOrdersAsync();
            return View(orders);
        }

        public async Task<IActionResult> Details(int id)
        {
            var order = await _orderService.GetOrderByIdAsync(id);
            if (order == null) return NotFound();
            return View(order);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateStatus(int id, OrderStatus status, bool returnToIndex = false)
        {
            await _orderService.UpdateOrderStatusAsync(id, status);
            TempData["Success"] = "Cập nhật trạng thái đơn hàng thành công!";
            if (returnToIndex) return RedirectToAction(nameof(Index));
            return RedirectToAction(nameof(Details), new { id = id });
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var order = await _orderService.GetOrderByIdAsync(id);
            if (order == null) return NotFound();
            
            // Restricted editing: Cannot edit if already shipping or beyond
            if (order.Status == OrderStatus.Shipping || order.Status == OrderStatus.Completed || order.Status == OrderStatus.Cancelled)
            {
                TempData["Error"] = "Không thể chỉnh sửa đơn hàng ở trạng thái này.";
                return RedirectToAction(nameof(Index));
            }
            
            return View(order);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EcommerceApp.Application.DTOs.Order.OrderDto model)
        {
            await _orderService.UpdateOrderAsync(model.Id, model);
            TempData["Success"] = "Cập nhật thông tin đơn hàng thành công!";
            return RedirectToAction(nameof(Index));
        }
    }
}
