using EcommerceApp.Application.Common;
using EcommerceApp.Application.Interfaces.Repositories;
using EcommerceApp.Application.Interfaces.Services;
using EcommerceApp.Application.Interfaces.UnitOfWork;

namespace EcommerceApp.Application.Features.Auth.RefreshToken
{
    public class RefreshTokenHandler
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRefreshTokenRepository _refreshTokenRepository;
        private readonly IJwtService _jwtService;
        private readonly IUserRepository _userRepository;

        public RefreshTokenHandler(
            IUnitOfWork unitOfWork,
            IRefreshTokenRepository refreshTokenRepository,
            IJwtService jwtService,
            IUserRepository userRepository)
        {
            _unitOfWork = unitOfWork;
            _refreshTokenRepository = refreshTokenRepository;
            _jwtService = jwtService;
            _userRepository = userRepository;
        }

        public async Task<ApiResponses<RefreshTokenResponse>> HandleAsync(
            RefreshTokenRequest request,
            CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(request.RefreshToken))
                return ApiResponses<RefreshTokenResponse>.Fail("Refresh token is required");

            var tokenHash = _jwtService.HashToken(request.RefreshToken);
            var refreshToken = await _refreshTokenRepository
                .GetByTokenHashAsync(tokenHash, cancellationToken);

            if (refreshToken == null)
                return ApiResponses<RefreshTokenResponse>.Fail("Invalid refresh token");

            if (refreshToken.ExpiresAt < System.DateTime.UtcNow)
                return ApiResponses<RefreshTokenResponse>.Fail("Refresh token has expired");

            if (refreshToken.IsRevoked)
                return ApiResponses<RefreshTokenResponse>.Fail("Refresh token has been revoked");
            var user = refreshToken.User;
            if (user is null || !user.IsActive)
                return ApiResponses<RefreshTokenResponse>.Fail("User account is inactive");

            var roles = user.UserRoles
                .Select(ur => ur.Role.Name)
                .ToList();

            var newAccessToken = _jwtService.GenerateAccessTroken(user, roles);
            var newRefreshToken = _jwtService.GenerateRefreshToken();
            var newTokenHash = _jwtService.HashToken(newRefreshToken);

            refreshToken.ReplacedByTokenHash = newTokenHash;
            _refreshTokenRepository.Update(refreshToken);

            var newRefreshTokenEntity = new Models.Entities.RefreshToken
            {
                Id = Guid.NewGuid(),
                UserId = user.Id,
                TokenHash = newTokenHash,
                ExpiresAt = System.DateTime.UtcNow.AddDays(7),
                CreatedAt = System.DateTime.UtcNow
            };

            await _refreshTokenRepository.AddAsync(newRefreshTokenEntity, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return ApiResponses<RefreshTokenResponse>.Ok(
               new RefreshTokenResponse
               {
                   AccessToken = newAccessToken,
                   RefreshToken = newRefreshToken
               },
               "Token refreshed successfully");
        }
    }
}
