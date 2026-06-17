using EcommerceApp.Base;
using EcommerceApp.Enums;
using EcommerceApp.Models.Entities;

namespace EcommerceApp.Models
{
    public class Notification : BaseEntity
    {
        public string Title { get; set; }
        public string Message { get; set; }
        public string? RedirectUrl { get; set; }
        public bool IsRead { get; set; } = false;
        public NotificationType Type { get; set; }
        public Guid UserId { get; set; }
        public User User { get; set; }
    }
}
