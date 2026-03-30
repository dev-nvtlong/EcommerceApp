using EcommerceApp.Enums;

namespace EcommerceApp.Application.DTOs.Account
{
    public class UserDto
    {
        public int Id { get; set; }
        public string? Email { get; set; }
        public string? UserName { get; set; }
        public string? FullName { get; set; }
        public string? Address { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Avatar { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public GenderType Gender { get; set; }
        public DateTime? CreateAt { get; set; }
        public IList<string>? Roles { get; set; }
    }
}
