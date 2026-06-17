using Azure.Core;
using EcommerceApp.Application.Common;
using EcommerceApp.Application.Interfaces.Repositories;
using EcommerceApp.Application.Interfaces.Services;
using EcommerceApp.Application.Interfaces.UnitOfWork;

namespace EcommerceApp.Application.Features.Auth.Login
{
    public class LoginHandler
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IRefreshTokenRepository _refreshTokenRepository;
        private readonly IJwtService _jwtService;

        public LoginHandler(
            IUnitOfWork unitOfWork,
            IUserRepository userRepository,
            IPasswordHasher passwordHasher,
            IRefreshTokenRepository refreshTokenRepository,
            IJwtService jwtService)
        {
            _unitOfWork = unitOfWork;
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
            _refreshTokenRepository = refreshTokenRepository;
            _jwtService = jwtService;
        }
        
        public async Task<ApiResponses<LoginResponse>> HandleAsync(
            LoginRequest request,
            CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(request.Email))
                return ApiResponses<LoginResponse>.Fail("Email is required");

            if (string.IsNullOrWhiteSpace(request.Password))
                return ApiResponses<LoginResponse>.Fail("Password is required");

            var user = await _userRepository.GetByEmailAsync(
                request.Email.Trim().ToLower(),
                cancellationToken);

            if (user == null)
                return ApiResponses<LoginResponse>.Fail("Invalid email or password");

            var isPasswordValid = _passwordHasher.Verify(
                request.Password,
                user.PasswordHash);

            if (!isPasswordValid)
                return ApiResponses<LoginResponse>.Fail("Invalid email or password");

            if (!user.IsActive)
                return ApiResponses<LoginResponse>.Fail("User account is inactive");

            var role = user.UserRoles
                .Select(ur => ur.Role.Name)
                .ToList();

            var accessToken = _jwtService.GenerateAccessTroken(user, role);
            var refreshToken = _jwtService.GenerateRefreshToken();
            var tokenHash = _jwtService.HashToken(refreshToken);

            var refreshTokenEntity = new Models.Entities.RefreshToken
            {
                Id = Guid.NewGuid(),
                UserId = user.Id,
                TokenHash = tokenHash,
                ExpiresAt = DateTime.UtcNow.AddDays(7)
            };

            await _refreshTokenRepository.AddAsync(refreshTokenEntity, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return ApiResponses<LoginResponse>.Ok(
                new LoginResponse
                {
                    AccessToken = accessToken,
                    RefreshToken = refreshToken,
                    UserId = user.Id,
                    Email = user.Email,
                    Roles = role
                },
                "Login successfully");
        }
    }
}
