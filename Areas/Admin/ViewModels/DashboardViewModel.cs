using EcommerceApp.Application.DTOs.Order;
using EcommerceApp.Application.DTOs.Notification;
using EcommerceApp.Application.DTOs.Product;

namespace EcommerceApp.Areas.Admin.ViewModels
{
    public class DashboardViewModel
    {
        public decimal TotalRevenue { get; set; }
        public decimal RevenueToday { get; set; }
        public decimal RevenueThisMonth { get; set; }
        public decimal RevenueThisYear { get; set; }
        public decimal RevenueFiltered { get; set; } // For custom filter
        
        public int TotalOrders { get; set; }
        public int TotalCustomers { get; set; }
        public int TotalProducts { get; set; }
        public int TotalOutOfStock { get; set; } // Out of stock count
        public int TotalReviews { get; set; }
        public int TotalComments { get; set; }
        public int TotalLikes { get; set; }
        
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        public List<OrderDto> RecentOrders { get; set; } = new();
        public List<NotificationDto> RecentNotifications { get; set; } = new();

        public List<ProductDto> LowStockProducts { get; set; } = new();
        public List<TopProductViewModel> TopSellingProducts { get; set; } = new();
        public List<ChartDataPoint> RevenueChartData { get; set; } = new();
    }

    public class TopProductViewModel
    {
        public Guid ProductId { get; set; }
        public string Name { get; set; }
        public string Thumbnail { get; set; }
        public int SalesCount { get; set; }
        public decimal Revenue { get; set; }
    }

    public class ChartDataPoint
    {
        public string Label { get; set; }
        public decimal Value { get; set; }
    }
}
