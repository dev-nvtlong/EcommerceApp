using EcommerceApp.Application.Interfaces.Repositories;
using EcommerceApp.Data;
using EcommerceApp.Models;
using Microsoft.EntityFrameworkCore;

namespace EcommerceApp.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly ApplicationDbContext _context;
        public OrderRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<Order> CreateAsync(Order order)
        {
            _context.Orders.Add(order);
            await _context.SaveChangesAsync();
            return order;
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await _context.Orders.FindAsync(id);
            if (entity != null)
            {
                _context.Orders.Remove(entity);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<List<Order>> GetAllByUserIdAsync(int userId)
        {
            return await _context.Orders
                .Include(o => o.Details)
                .Where(o => o.UserId == userId)
                .ToListAsync();
        }

        public async Task<Order?> GetByIdAsync(int id)
        {
            return await _context.Orders
                .Include(o => o.User)
                .Include(o => o.Details)
                    .ThenInclude(d => d.Product)
                .FirstOrDefaultAsync(o => o.ID == id);
        }

        public async Task UpdateAsync(Order order)
        {
            _context.Orders.Update(order);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Order>> GetAllWithUserAsync()
        {
            return await _context.Orders
                .Include(o => o.User)
                .Include(o => o.Details)
                .OrderByDescending(o => o.OrderDate)
                .ToListAsync();
        }
    }
}
