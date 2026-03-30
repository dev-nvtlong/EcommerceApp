using EcommerceApp.Application.DTOs.Order;
using EcommerceApp.Application.DTOs.Notification;

namespace EcommerceApp.Areas.Admin.ViewModels
{
    public class DashboardViewModel
    {
        public decimal TotalRevenue { get; set; }
        public int TotalOrders { get; set; }
        public int TotalCustomers { get; set; }
        public int TotalProducts { get; set; }
        public int TotalReviews { get; set; }
        public int TotalComments { get; set; }
        public int TotalLikes { get; set; }
        
        public List<OrderDto> RecentOrders { get; set; } = new();
        public List<NotificationDto> RecentNotifications { get; set; } = new();
    }
}
