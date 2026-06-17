namespace EcommerceApp.Application.Features.Auth.Register
{
    public class RegisterResponse
    {
        public Guid UserId { get; set; }
        public string Email { get; set; } = string.Empty;
    }
}
