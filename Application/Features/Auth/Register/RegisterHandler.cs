using EcommerceApp.Application.Common;
using EcommerceApp.Application.Interfaces.Repositories;
using EcommerceApp.Application.Interfaces.Services;
using EcommerceApp.Application.Interfaces.UnitOfWork;
using EcommerceApp.Models.Entities;

namespace EcommerceApp.Application.Features.Auth.Register
{
    public class RegisterHandler
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IUnitOfWork _unitOfWork;
        public RegisterHandler(
            IUserRepository userRepository,
            IPasswordHasher passwordHasher,
            IUnitOfWork unitOfWork)
        {
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
            _unitOfWork = unitOfWork;
        }

        public async Task<ApiResponses<RegisterResponse>> HandleAsync(
            RegisterRequest request,
            CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(request.Email))
                return ApiResponses<RegisterResponse>.Fail("Email is required");

            if (string.IsNullOrWhiteSpace(request.Password))
                return ApiResponses<RegisterResponse>.Fail("Password is required");

            var exists = await _userRepository.ExistsByEmailAsync(
                request.Email,
                cancellationToken);

            if (exists)
                return ApiResponses<RegisterResponse>.Fail("Email already exists");

            DateOnly? dateOfBirth = null;
            if (!string.IsNullOrWhiteSpace(request.DateOfBirth))
            {
                if (!DateOnly.TryParse(request.DateOfBirth, out var parsedDateOfBirth))
                    return ApiResponses<RegisterResponse>.Fail("Date of birth is invalid");

                dateOfBirth = parsedDateOfBirth;
            }

            var user = new User
            {
                Id = Guid.NewGuid(),
                Email = request.Email.Trim().ToLower(),
                PasswordHash = _passwordHasher.Hash(request.Password),
                IsActive = true,
                IsEmailConfirmed = false,
                CreatedAt = DateTime.UtcNow
            };

            await _userRepository.AddAsync(user, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return ApiResponses<RegisterResponse>.Ok(
                new RegisterResponse
                {
                    UserId = user.Id,
                    Email = user.Email
                },
                "Register successfully");
        }
    }
}
