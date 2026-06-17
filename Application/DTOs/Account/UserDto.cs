using EcommerceApp.Enums;
using System.ComponentModel.DataAnnotations;

namespace EcommerceApp.Application.DTOs.Account
{
    public class UserDto
    {
        public Guid Id { get; set; }
        public string Email { get; set; } = null!;
        public string FullName { get; set; } = null!;
        public string? Address { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Avatar { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public GenderType Gender { get; set; }
        public DateTime? CreatedAt { get; set; } = DateTime.UtcNow;
        public IList<string>? Roles { get; set; }
    }
}
