using EcommerceApp.Application.DTOs.Notification;
using EcommerceApp.Enums;

namespace EcommerceApp.Application.Interfaces.Services
{
    public interface INotificationService
    {
        Task<List<NotificationDto>> GetRecentNotificationsAsync(int count = 10);
        Task<int> GetUnreadCountAsync();
        Task<bool> MarkAsReadAsync(int id);
        Task<bool> MarkAllAsReadAsync();
        Task CreateNotificationAsync(string title, string message, NotificationType type, string? redirectUrl = null);
    }
}
