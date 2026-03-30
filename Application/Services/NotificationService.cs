using AutoMapper;
using EcommerceApp.Application.DTOs.Notification;
using EcommerceApp.Application.Interfaces.Services;
using EcommerceApp.Data;
using EcommerceApp.Enums;
using EcommerceApp.Models;
using Microsoft.EntityFrameworkCore;

namespace EcommerceApp.Application.Services
{
    public class NotificationService : INotificationService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public NotificationService(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<NotificationDto>> GetRecentNotificationsAsync(int count = 10)
        {
            var notifications = await _context.Notifications
                .OrderByDescending(n => n.CreatedAt)
                .Take(count)
                .ToListAsync();

            return _mapper.Map<List<NotificationDto>>(notifications);
        }

        public async Task<int> GetUnreadCountAsync()
        {
            return await _context.Notifications.CountAsync(n => !n.IsRead);
        }

        public async Task<bool> MarkAsReadAsync(int id)
        {
            var notification = await _context.Notifications.FindAsync(id);
            if (notification == null) return false;

            notification.IsRead = true;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> MarkAllAsReadAsync()
        {
            var unread = await _context.Notifications.Where(n => !n.IsRead).ToListAsync();
            foreach (var n in unread) n.IsRead = true;
            
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task CreateNotificationAsync(string title, string message, NotificationType type, string? redirectUrl = null)
        {
            var notification = new Notification
            {
                Title = title,
                Message = message,
                Type = type,
                RedirectUrl = redirectUrl,
                CreatedAt = DateTime.Now
            };

            _context.Notifications.Add(notification);
            await _context.SaveChangesAsync();
        }
    }
}
