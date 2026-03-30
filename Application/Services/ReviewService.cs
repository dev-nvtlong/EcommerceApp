using AutoMapper;
using EcommerceApp.Application.DTOs.Review;
using EcommerceApp.Application.Interfaces.Services;
using EcommerceApp.Data;
using EcommerceApp.Models;
using Microsoft.EntityFrameworkCore;

namespace EcommerceApp.Application.Services
{
    public class ReviewService : IReviewService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly INotificationService _notificationService;

        public ReviewService(ApplicationDbContext context, IMapper mapper, INotificationService notificationService)
        {
            _context = context;
            _mapper = mapper;
            _notificationService = notificationService;
        }

        public async Task<List<ReviewDto>> GetByProductIdAsync(int productId)
        {
            var reviews = await _context.Reviews
                .Include(r => r.User)
                .Where(r => r.ProductId == productId)
                .OrderByDescending(r => r.CreatedAt)
                .ToListAsync();

            return _mapper.Map<List<ReviewDto>>(reviews);
        }

        public async Task<bool> AddReviewAsync(int userId, int productId, int rating, string comment)
        {
            var review = new Review
            {
                UserId = userId,
                ProductId = productId,
                Rating = rating,
                Comment = comment,
                CreatedAt = DateTime.Now
            };

            _context.Reviews.Add(review);
            await _context.SaveChangesAsync();

            // Notify Admin
            var user = await _context.Users.FindAsync(userId);
            var product = await _context.Products.FindAsync(productId);
            await _notificationService.CreateNotificationAsync(
                "Đánh giá mới",
                $"{user?.FullName ?? user?.UserName} đã đánh giá {rating} sao cho sản phẩm: {product?.Name}",
                Enums.NotificationType.NewReview,
                $"/Admin/Review"
            );

            return true;
        }

        public async Task<List<ReviewDto>> GetAllAsync()
        {
            var reviews = await _context.Reviews
                .Include(r => r.Product)
                .Include(r => r.User)
                .OrderByDescending(r => r.CreatedAt)
                .ToListAsync();

            return _mapper.Map<List<ReviewDto>>(reviews);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var review = await _context.Reviews.FindAsync(id);
            if (review == null) return false;

            _context.Reviews.Remove(review);
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
