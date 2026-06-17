using EcommerceApp.Base;

namespace EcommerceApp.Models.Entities
{
    public class User : BaseEntity
    {
        public string Email { get; set; } = null!;
        public string? UserName { get; set; }
        public string PasswordHash { get; set; } = null!;
        public bool IsEmailConfirmed { get; set; } = false;
        public bool IsActive { get; set; } = true;
        public DateTime? LastLoginAt { get; set; }
        public ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
        public ICollection<RefreshToken> RefreshTokens { get; set; } = new List<RefreshToken>();
        public ICollection<Notification> Notifications { get; set; } = new List<Notification>();
        public ICollection<Order> Orders { get; set; } = new List<Order>();
        public ICollection<Review> Reviews { get; set; } = new List<Review>();
        public ICollection<Comment> Comments { get; set; } = new List<Comment>();
        public ICollection<Like> Likes { get; set; } = new List<Like>();
        public ICollection<BlogPost> BlogPosts { get; set; } = new List<BlogPost>();
        public UserProfile Profile { get; set; } = null!;

    }
}
