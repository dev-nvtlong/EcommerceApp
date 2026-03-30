using EcommerceApp.Areas.Admin.ViewModels;
using EcommerceApp.Data;
using EcommerceApp.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using EcommerceApp.Application.DTOs.Order;
using Microsoft.AspNetCore.Mvc;

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

        public async Task<IActionResult> Index()
        {
            var viewModel = new DashboardViewModel
            {
                TotalRevenue = await _context.Orders
                    .Where(o => o.Status == Enums.OrderStatus.Completed)
                    .SumAsync(o => o.TotalAmount),
                TotalOrders = await _context.Orders.CountAsync(),
                TotalCustomers = await _context.Users.CountAsync(),
                TotalProducts = await _context.Products.CountAsync(),
                TotalReviews = await _context.Reviews.CountAsync(),
                TotalComments = await _context.Comments.CountAsync(),
                TotalLikes = await _context.Likes.CountAsync(),
                
                RecentOrders = await _orderService.GetAllOrdersAsync(), // We can limit this later or in the service
                RecentNotifications = await _notificationService.GetRecentNotificationsAsync(5)
            };

            // Limit recent orders to 5
            viewModel.RecentOrders = viewModel.RecentOrders.Take(5).ToList();

            return View(viewModel);
        }
    }
}
