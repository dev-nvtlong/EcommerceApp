using EcommerceApp.Application.Interfaces.Repositories;
using EcommerceApp.Data;
using EcommerceApp.Models;
using Microsoft.EntityFrameworkCore;

namespace EcommerceApp.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly ApplicationDbContext _context;
        public ProductRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task CreateAsync(Product product)
        {
            await _context.Products.AddAsync(product);
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await _context.Products.FindAsync(id);
            if (entity != null)
            {
                _context.Products.Remove(entity);
            }
        }

        public async Task<List<Product>> GetAllAsync()
        {
            return await _context.Products.Include(p => p.Images).ToListAsync();
        }

        public async Task<Product?> GetByIdAsync(int id)
        {
            return await _context.Products.Include(p => p.Images).FirstOrDefaultAsync(p => p.ID == id);
        }

        public async Task<Product?> GetByIdAsNoTrackingAsync(int id)
        {
            return await _context.Products.AsNoTracking().Include(p => p.Images).FirstOrDefaultAsync(p => p.ID == id);
        }

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Product product)
        {
            _context.Products.Update(product);
            await _context.SaveChangesAsync();
        }
    }
}
