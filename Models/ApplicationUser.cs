using EcommerceApp.Enums;
using Microsoft.AspNetCore.Identity;

namespace EcommerceApp.Models
{
    public class ApplicationUser : IdentityUser<int>
    {
        public string? FullName { get; set; }
        public string? Address { get; set; }
        public string? Avatar { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public GenderType Gender { get; set; } = GenderType.Unknown;

        public DateTime? CreateAt { get; set; }
        public int? CreateByUserID { get; set; }
        public DateTime? ModifiedAt { get; set; }
        public int? ModifiedByUserID { get;set; }

        // Quan hệ: Một người 
    }
}
