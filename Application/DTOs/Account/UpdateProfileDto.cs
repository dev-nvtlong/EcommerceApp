using EcommerceApp.Enums;
using System.ComponentModel.DataAnnotations;

namespace EcommerceApp.Application.DTOs.Account
{
    public class UpdateProfileDto
    {
        [Required(ErrorMessage = "Vui lòng nhập họ tên")]
        public string FullName { get; set; }

        public string? PhoneNumber { get; set; }

        public string? Address { get; set; }

        public DateTime? DateOfBirth { get; set; }

        public GenderType Gender { get; set; }

        public string? Avatar { get; set; }
    }
}
