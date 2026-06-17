using EcommerceApp.Application.Common;
using EcommerceApp.Application.DTOs.Account;
using EcommerceApp.Application.Features.Auth.Login;
using EcommerceApp.Models;
using EcommerceApp.Models.Entities;
using EcommerceApp.Models.ViewModels;

namespace EcommerceApp.Application.Interfaces.Services
{
    public interface IUserService
    {
        Task<(bool Success, string Message, string? Token, UserDto? User)> RegisterAsync(string fullName, string email, string password);
        Task<ApiResponses<LoginResponse>> LoginAsync(LoginRequest loginRequest, CancellationToken cancellationToken);
        Task<(bool Success, string Message, string? Token, User? User)> AdminLoginAsync(string email, string password);
        Task<UserDto?> GetProfileAsync(Guid userId);
        Task<bool> UpdateProfileAsync(Guid userId, UpdateProfileDto model);
        Task<bool> UpdateAvatarAsync(Guid userId, IFormFile avatarFile);
        Task<List<UserDto>> GetAllUsersAsync();
        Task<UserDto?> GetUserByIdAsync(Guid id);
        Task<bool> DeleteUserAsync(Guid id);
    }
}
