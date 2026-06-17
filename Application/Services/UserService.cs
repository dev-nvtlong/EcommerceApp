using AutoMapper;
using EcommerceApp.Application.Common;
using EcommerceApp.Application.DTOs.Account;
using EcommerceApp.Application.Features.Auth.Login;
using EcommerceApp.Application.Interfaces.Repositories;
using EcommerceApp.Application.Interfaces.Services;
using EcommerceApp.Data;
using EcommerceApp.Models.Entities;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace EcommerceApp.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public UserService(IUserRepository userRepository, ApplicationDbContext context, IMapper mapper)
        {
            _userRepository = userRepository;
            _context = context;
            _mapper = mapper;
        }

        // ======= Profile ====
        public async Task<UserDto?> GetProfileAsync(Guid userId)
        {
            var profile = await _userRepository.GetProfileByUserIdAsync(userId);
            if (profile == null)
            {
                var user = await _userRepository.GetByIdAsync(userId);
                if (user == null) return null;

                profile = new UserProfile
                {
                    UserId = userId,
                    Email = user.Email,
                    FullName = user.UserName ?? user.Email
                };
                await _userRepository.AddProfileAsync(profile);
                await _context.SaveChangesAsync();
            }
            var userDto = _mapper.Map<UserDto>(profile);
            return userDto;
        }

        public async Task<UserDto?> GetUserByIdAsync(Guid id)
        {        
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null)
            {
                return null;
            }
            var userDto = _mapper.Map<UserDto>(user);
            return userDto;
        }

        public async Task<List<UserDto>> GetAllUsersAsync()
        {
            var users = await _userRepository.GetAllUsersAsync();
            return _mapper.Map<List<UserDto>>(users);
        }


        // ====== Cập nhật profile =====
        public async Task<bool> UpdateProfileAsync(Guid userId, UpdateProfileDto model)
        {
            var profile = await _userRepository.GetProfileByUserIdAsync(userId);

            if (profile == null)
            {
                var user = await _userRepository.GetByIdAsync(userId);
                if (user == null) return false;

                profile = new UserProfile
                {
                    UserId = userId,
                    Email = user.Email
                };
                await _userRepository.AddProfileAsync(profile);
            }

            if (model.FullName != null)
                profile.FullName = model.FullName;

            if (model.PhoneNumber != null)
                profile.PhoneNumber = model.PhoneNumber;

            if (model.DateOfBirth.HasValue)
                profile.DateOfBirth = DateOnly.FromDateTime(model.DateOfBirth.Value);

            profile.Gender = model.Gender;

            profile.ModifiedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return true;
        }

        // ====== Cập nhật avatar =====
        public async Task<bool> UpdateAvatarAsync(Guid userId, IFormFile avatarFile)
        {
            if (avatarFile == null || avatarFile.Length == 0)
                return false;

            var uploadsFolder = Path.Combine(
                Directory.GetCurrentDirectory(), "wwwroot", "uploads", "avatars");

            Directory.CreateDirectory(uploadsFolder);

            var fileName = $"{userId}_{Guid.NewGuid()}{Path.GetExtension(avatarFile.FileName)}";

            var filePath = Path.Combine(uploadsFolder, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
                await avatarFile.CopyToAsync(stream);

            var profile = await _userRepository.GetProfileByUserIdAsync(userId);

            if (profile == null)
            {
                var user = await _userRepository.GetByIdAsync(userId);
                if (user == null) return false;

                profile = new UserProfile
                {
                    UserId = userId,
                    Email = user.Email
                };
                _userRepository.AddProfileAsync(profile).Wait();
            }

            profile.AvatarUrl = $"/uploads/avatars/{fileName}";

            await _context.SaveChangesAsync();
            return true;
        }

        // --- Chưa cần thiết, để lại stub ---
        public async Task<ApiResponses<LoginResponse>> LoginAsync(
            LoginRequest loginRequest, 
            CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByEmailAsync(
                loginRequest.Email, 
                cancellationToken);

            if (user == null)
            {
                return ApiResponses<LoginResponse>.Fail(
                    "Tài khoản không tồn tại");
            }

            if (!BCrypt.Net.BCrypt.Verify(
                loginRequest.Password,
                user.PasswordHash))
            {
                return ApiResponses<LoginResponse>.Fail(
                    "Mật khẩu không đúng");
            }

            var claims = new List<Claim>
            {
                new Claim(
                    ClaimTypes.NameIdentifier,
                    user.Id.ToString()),

                new Claim(
                    ClaimTypes.Email,
                    user.Email)
            };

            foreach (var role in user.UserRoles) {
                claims.Add(
                    new Claim(
                        ClaimTypes.Role,
                        role.Role.Name));
            }

            var response = new LoginResponse
            {
                UserId = user.Id,
                Email = user.Email,
                Roles = user.UserRoles
                    .Select(x => x.Role.Name)
                    .ToList()
            };

            return ApiResponses<LoginResponse>.Ok(
                response,
                "Đăng nhập thành công");
        }

        public Task<(bool Success, string Message, string? Token, UserDto? User)> RegisterAsync(string fullName, string email, string password)
            => throw new NotImplementedException();

        public Task<(bool Success, string Message, string? Token, User? User)> AdminLoginAsync(string email, string password)
            => throw new NotImplementedException();

        public async Task<bool> DeleteUserAsync(Guid id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null)
            {
                return false;
            }
            
            await _userRepository.DeleteAsync(user);
            return true;
        }
    }
}
