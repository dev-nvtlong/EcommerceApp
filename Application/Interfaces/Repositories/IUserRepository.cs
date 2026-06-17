using EcommerceApp.Models.Entities;

namespace EcommerceApp.Application.Interfaces.Repositories
{
    public interface IUserRepository
    {
        Task<IEnumerable<User>> GetAllUsersAsync(CancellationToken cancellationToken = default);
        Task<User?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default);
        Task<bool> ExistsByEmailAsync(string email, CancellationToken cancellationToken = default);
        Task AddAsync(User user, CancellationToken cancellationToken = default);
        void Update(User user);
        Task DeleteAsync(User user, CancellationToken cancellationToken = default);

        Task<UserProfile?> GetProfileByUserIdAsync(Guid userId, CancellationToken cancellationToken = default);
        Task AddProfileAsync(UserProfile profile, CancellationToken cancellationToken = default);
        void UpdateProfile(UserProfile profile);
    }
}
