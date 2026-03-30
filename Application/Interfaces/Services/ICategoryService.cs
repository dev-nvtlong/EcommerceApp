using EcommerceApp.Application.DTOs.Category;

namespace EcommerceApp.Application.Interfaces.Services
{
    public interface ICategoryService
    {
        Task<List<CategoryDto>> GetAllAsync();
        Task<CategoryDto?> GetByIdAsync(int id);
        Task CreateAsync(CategoryDto dto);
        Task UpdateAsync(CategoryDto dto);
        Task DeleteAsync(int id);
    }
}
