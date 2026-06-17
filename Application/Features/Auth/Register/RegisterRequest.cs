namespace EcommerceApp.Application.Features.Auth.Register
{
    public class RegisterRequest
    {
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string? DateOfBirth { get; set; } = string.Empty;
        public string? FullName { get; set; } = string.Empty;
        public string? PhoneNumber { get; set; } = string.Empty;
        public string? Gender { get; set; }
        public string? AvatarUrl { get; set; }
    }
}
