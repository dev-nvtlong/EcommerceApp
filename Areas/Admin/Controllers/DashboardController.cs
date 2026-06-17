using EcommerceApp.Areas.Admin.ViewModels;
using EcommerceApp.Data;
using EcommerceApp.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using EcommerceApp.Application.DTOs.Order;
using Microsoft.AspNetCore.Mvc;
using EcommerceApp.Application.DTOs.Product;
using EcommerceApp.Models;

namespace EcommerceApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class DashboardController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IOrderService _orderService;
        private readonly INotificationService _notificationService;
        private readonly IMapper _mapper;

        public DashboardController(
            ApplicationDbContext context, 
            IOrderService orderService, 
            INotificationService notificationService,
            IMapper mapper)
        {
            _context = context;
            _orderService = orderService;
            _notificationService = notificationService;
            _mapper = mapper;
        }

        public async Task<IActionResult> Index(DateTime? startDate, DateTime? endDate)
        {
            var now = DateTime.Now;
            var startOfDay = now.Date;
            var startOfMonth = new DateTime(now.Year, now.Month, 1);
            var startOfYear = new DateTime(now.Year, 1, 1);

            var orders = _context.Orders.Where(o => o.Status == Enums.OrderStatus.Completed);

            var viewModel = new DashboardViewModel
            {
                TotalRevenue = await orders.SumAsync(o => o.TotalAmount),
                RevenueToday = await orders.Where(o => o.OrderDate >= startOfDay).SumAsync(o => o.TotalAmount),
                RevenueThisMonth = await orders.Where(o => o.OrderDate >= startOfMonth).SumAsync(o => o.TotalAmount),
                RevenueThisYear = await orders.Where(o => o.OrderDate >= startOfYear).SumAsync(o => o.TotalAmount),

                TotalOrders = await _context.Orders.CountAsync(),
                TotalCustomers = await _context.Users.CountAsync(),
                TotalProducts = await _context.Products.CountAsync(),
                TotalOutOfStock = await _context.Products.CountAsync(p => p.StockQuantity <= 0),
                TotalReviews = await _context.Reviews.CountAsync(),
                TotalComments = await _context.Comments.CountAsync(),
                TotalLikes = await _context.Likes.CountAsync(),
                
                RecentOrders = await _orderService.GetAllOrdersAsync(),
                RecentNotifications = await _notificationService.GetRecentNotificationsAsync(5),

                LowStockProducts = (await _context.Products
                    .Include(p => p.Images)
                    .Where(p => p.StockQuantity < 10)
                    .OrderBy(p => p.StockQuantity)
                    .Take(5)
                    .ToListAsync())
                    .Select(p => new ProductDto { 
                        ProductId = p.Id, 
                        Name = p.Name, 
                        StockQuantity = p.StockQuantity, 
                        Price = p.Price,
                        ImageUrls = p.Images?.Select(i => i.ImageUrl).ToList()
                    }).ToList(),

                TopSellingProducts = await _context.OrderDetails
                    .GroupBy(oi => new { oi.ProductId, oi.Product.Name })
                    .Select(g => new TopProductViewModel
                    {
                        ProductId = g.Key.ProductId,
                        Name = g.Key.Name,
                        Thumbnail = _context.Set<ProductImage>()
                            .Where(pi => pi.ProductId == g.Key.ProductId)
                            .OrderByDescending(pi => pi.IsMain)
                            .Select(pi => pi.ImageUrl)
                            .FirstOrDefault() ?? "",
                        SalesCount = g.Sum(oi => oi.Quantity),
                        Revenue = g.Sum(oi => oi.Quantity * oi.Price)
                    })
                    .OrderByDescending(p => p.SalesCount)
                    .Take(5)
                    .ToListAsync(),

                StartDate = startDate,
                EndDate = endDate
            };

            // Custom Filtered Revenue
            if (startDate.HasValue)
            {
                var filtered = orders.Where(o => o.OrderDate >= startDate.Value);
                if (endDate.HasValue)
                {
                    var trueEnd = endDate.Value.Date.AddDays(1);
                    filtered = filtered.Where(o => o.OrderDate < trueEnd);
                }
                viewModel.RevenueFiltered = await filtered.SumAsync(o => o.TotalAmount);
            }

            // Chart Data: Last 7 days
            for (int i = 6; i >= 0; i--)
            {
                var date = now.Date.AddDays(-i);
                var revenue = await orders
                    .Where(o => o.OrderDate >= date && o.OrderDate < date.AddDays(1))
                    .SumAsync(o => o.TotalAmount);

                viewModel.RevenueChartData.Add(new ChartDataPoint
                {
                    Label = date.ToString("dd/MM"),
                    Value = revenue
                });
            }

            // Limit recent orders to 5
            viewModel.RecentOrders = viewModel.RecentOrders.Take(5).ToList();

            return View(viewModel);
        }
    }
}
