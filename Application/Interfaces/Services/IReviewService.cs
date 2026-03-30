using EcommerceApp.Application.DTOs.Review;

namespace EcommerceApp.Application.Interfaces.Services
{
    public interface IReviewService
    {
        Task<List<ReviewDto>> GetByProductIdAsync(int productId);
        Task<bool> AddReviewAsync(int userId, int productId, int rating, string comment);
        Task<List<ReviewDto>> GetAllAsync();
        Task<bool> DeleteAsync(int id);
    }
}
