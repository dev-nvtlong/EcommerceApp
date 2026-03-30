using AutoMapper;
using EcommerceApp.Application.DTOs.Category;
using EcommerceApp.Application.Interfaces.Repositories;
using EcommerceApp.Application.Interfaces.Services;
using EcommerceApp.Models;

namespace EcommerceApp.Application.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;
        public CategoryService(ICategoryRepository categoryRepository, IMapper mapper)
        {
            _categoryRepository = categoryRepository;
            _mapper = mapper;
        }

        public async Task CreateAsync(CategoryDto dto)
        {
            var entity = _mapper.Map<Category>(dto);
            await _categoryRepository.CreateAsync(entity);
            await _categoryRepository.SaveAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await _categoryRepository.GetByIdAsync(id);
            if (entity != null)
            {
                await _categoryRepository.DeleteAsync(id);
                await _categoryRepository.SaveAsync();
            }
        }

        public async Task<List<CategoryDto>> GetAllAsync()
        {
            var data = await _categoryRepository.GetAllAsync();
            return _mapper.Map<List<CategoryDto>>(data);
        }

        public async Task<CategoryDto?> GetByIdAsync(int id)
        {
            var entity = await _categoryRepository.GetByIdAsync(id);
            return _mapper?.Map<CategoryDto?>(entity);
        }

        public async Task UpdateAsync(CategoryDto dto)
        {
            var entity = _mapper.Map<Category>(dto);
            await _categoryRepository.UpdateAsync(entity);
            await _categoryRepository.SaveAsync();
        }
    }
}
