using EcommerceApp.Models.Entities;

namespace EcommerceApp.Application.Interfaces.Repositories
{
    public interface IRoleRepository
    {
        Task<Role?> GetByNameAsync(string name, CancellationToken cancellationToken = default);
        Task<List<Role>> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default);
    }
}
