using EcommerceApp.Models.Entities;

namespace EcommerceApp.Application.Interfaces.Services
{
    public interface IJwtService
    {
        string GenerateAccessTroken(User user, IEnumerable<string> roles);
        string GenerateRefreshToken();
        string HashToken(string token);
    }
}
