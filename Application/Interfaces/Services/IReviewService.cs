using EcommerceApp.Application.DTOs.Review;

namespace EcommerceApp.Application.Interfaces.Services
{
    public interface IReviewService
    {
        Task<List<ReviewDto>> GetByProductIdAsync(Guid productId);
        Task<bool> AddReviewAsync(Guid userId, Guid productId, int rating, string comment);
        Task<List<ReviewDto>> GetAllAsync();
        Task<bool> DeleteAsync(Guid id);
    }
}
