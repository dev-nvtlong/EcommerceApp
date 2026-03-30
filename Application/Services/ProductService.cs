using AutoMapper;
using EcommerceApp.Application.DTOs.Product;
using EcommerceApp.Application.Interfaces.Repositories;
using EcommerceApp.Application.Interfaces.Services;
using EcommerceApp.Models;

namespace EcommerceApp.Application.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _repository;
        private readonly IMapper _mapper;
        public ProductService(IProductRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }
        public async Task CreateAsync(ProductDto dto)
        {
            var entity = _mapper.Map<Product>(dto);
            await _repository.CreateAsync(entity);
            await _repository.SaveAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await _repository.GetByIdAsync(id);
            if (entity != null)
            {
                await _repository.DeleteAsync(id);
                await _repository.SaveAsync();
            }
        }

        public async Task<List<ProductDto>> GetAllAsync()
        {
            var data = await _repository.GetAllAsync();
            return _mapper.Map<List<ProductDto>>(data);
        }

        public async Task<ProductDto?> GetByIdAsync(int id)
        {
            var entity = await _repository.GetByIdAsync(id);
            return _mapper.Map<ProductDto?>(entity);
        }

        public async Task<ProductDto?> GetByIdAsNoTrackingAsync(int id)
        {
            var entity = await _repository.GetByIdAsNoTrackingAsync(id);
            return _mapper.Map<ProductDto?>(entity);
        }

        public async Task UpdateAsync(ProductDto dto)
        {
            var existingProduct = await _repository.GetByIdAsync(dto.ProductId);
            if (existingProduct != null)
            {
                // Update basic properties
                _mapper.Map(dto, existingProduct);
                
                await _repository.UpdateAsync(existingProduct);
                await _repository.SaveAsync();
            }
        }
    }
}
