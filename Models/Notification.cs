using EcommerceApp.Base;
using EcommerceApp.Enums;

namespace EcommerceApp.Models
{
    public class Notification : AuditableEntity
    {
        public string Title { get; set; }
        public string Message { get; set; }
        public string? RedirectUrl { get; set; }
        public bool IsRead { get; set; } = false;
        public NotificationType Type { get; set; }
    }
}
