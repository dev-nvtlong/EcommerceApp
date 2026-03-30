using EcommerceApp.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EcommerceApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class NotificationController : Controller
    {
        private readonly INotificationService _notificationService;

        public NotificationController(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        [HttpGet]
        public async Task<IActionResult> GetRecent()
        {
            var notifications = await _notificationService.GetRecentNotificationsAsync();
            var unreadCount = await _notificationService.GetUnreadCountAsync();
            return Json(new { success = true, notifications, unreadCount });
        }

        [HttpPost]
        public async Task<IActionResult> MarkAsRead(int id)
        {
            var result = await _notificationService.MarkAsReadAsync(id);
            return Json(new { success = result });
        }

        [HttpPost]
        public async Task<IActionResult> MarkAllAsRead()
        {
            var result = await _notificationService.MarkAllAsReadAsync();
            return Json(new { success = result });
        }
    }
}
