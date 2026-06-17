using EcommerceApp.Base;
using EcommerceApp.Enums;

namespace EcommerceApp.Models.Entities
{
    public class UserProfile : BaseEntity
    {

        // Liên kết với AuthService
        public Guid UserId { get; set; }

        public string Email { get; set; } = null!;

        public string? FullName { get; set; }

        public string? PhoneNumber { get; set; }

        public string? AvatarUrl { get; set; }

        public DateOnly? DateOfBirth { get; set; }

        public GenderType Gender { get; set; }
        public User User { get; set; } = null!;
    }
}
