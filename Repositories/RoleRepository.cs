using EcommerceApp.Application.Interfaces.Repositories;
using EcommerceApp.Data;
using EcommerceApp.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace EcommerceApp.Repositories
{
    public class RoleRepository : IRoleRepository
    {
        private readonly ApplicationDbContext _context;
        public RoleRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<Role?> GetByNameAsync(string name, CancellationToken cancellationToken = default)
        {
            var normalizedName = name.ToUpper();

            return await _context.Roles
                .FirstOrDefaultAsync(r => r.NormalizedName == normalizedName, cancellationToken);
        }

        public async Task<List<Role>> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default)
        {
            return await _context.UserRoles
                .Where(ur => ur.UserId == userId)
                .Select(ur => ur.Role)
                .ToListAsync(cancellationToken);
        }
    }
}
