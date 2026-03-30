using EcommerceApp.Enums;

namespace EcommerceApp.Application.DTOs.Notification
{
    public class NotificationDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Message { get; set; }
        public string? RedirectUrl { get; set; }
        public bool IsRead { get; set; }
        public NotificationType Type { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
