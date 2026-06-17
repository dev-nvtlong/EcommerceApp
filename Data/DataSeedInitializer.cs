using EcommerceApp.Application.Interfaces.Services;
using EcommerceApp.Models.Entities;

namespace EcommerceApp.Data
{
    public class DataSeedInitializer
    {
        public static async Task ExecuteAsync(ApplicationDbContext context, IPasswordHasher passwordHasher)
        {
            await SeedRoleAsync(context);
            await SeedAdminUserAsync(context, passwordHasher);
        }
        private static async Task SeedRoleAsync(ApplicationDbContext context)
        {
            var roles = new[] { "Admin", "Customer" };

            foreach (var role in roles)
            {
                if (!context.Roles.Any(r => r.NormalizedName == role.ToUpperInvariant()))
                {
                    context.Roles.Add(new Role
                    {
                        Id = Guid.NewGuid(),
                        Name = role,
                        NormalizedName = role.ToUpperInvariant()
                    });
                }
            }

            await context.SaveChangesAsync();

        }

        private static async Task SeedAdminUserAsync(ApplicationDbContext context, IPasswordHasher passwordHasher)
        {
            const string adminEmail = "admin@vuoncay.com";

            if (context.Users.Any(u => u.Email == adminEmail))
                return;

            var adminId = Guid.Parse("11111111-1111-1111-1111-111111111111");
            var adminUser = new User
            {
                Id = adminId,
                Email = adminEmail,
                UserName = "admin",
                PasswordHash = passwordHasher.Hash("Admin@123"),
                IsEmailConfirmed = true,
                IsActive = true,
                CreatedAt = DateTime.UtcNow,
            };

            context.Users.Add(adminUser);
            await context.SaveChangesAsync();

            var adminRole = context.Roles.FirstOrDefault(r => r.NormalizedName == "ADMIN");
            if (adminRole != null)
            {
                context.UserRoles.Add(new UserRole
                {
                    UserId = adminUser.Id,
                    RoleId = adminRole.Id
                });
                await context.SaveChangesAsync();
            }
        }

    }
}
