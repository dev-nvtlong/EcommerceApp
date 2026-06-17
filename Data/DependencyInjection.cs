using EcommerceApp.Application;
using EcommerceApp.Application.Features.Auth.Login;
using EcommerceApp.Application.Features.Auth.RefreshToken;
using EcommerceApp.Application.Features.Auth.Register;
using EcommerceApp.Application.Interfaces.Repositories;
using EcommerceApp.Application.Interfaces.Services;
using EcommerceApp.Application.Interfaces.UnitOfWork;
using EcommerceApp.Application.Services;
using EcommerceApp.Models;
using EcommerceApp.Models.Entities;
using EcommerceApp.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace EcommerceApp.Data
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure( 
            this IServiceCollection services,
            IConfiguration configuration)
        {
            var connectionString =
               configuration.GetConnectionString("DefaultConnection");

            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(connectionString,
                    b => b.MigrationsAssembly(
                        typeof(ApplicationDbContext).Assembly.FullName)));
            #region ============== Đăng ký Repository =====================
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<ICartRepository, CartRepository>();
            services.AddScoped<IOrderRepository, OrderRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IRoleRepository, RoleRepository>();
            services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();
            #endregion

            #region ============== Đăng ký Service =====================
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<ICartService, CartService>();
            services.AddScoped<IOrderService, OrderService>();
            services.AddScoped<IBlogService, BlogService>();
            services.AddScoped<INotificationService, NotificationService>();
            services.AddScoped<IReviewService, ReviewService>();
            services.AddScoped<IPasswordHasher, PasswordHasher>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IJwtService, JwtService>();
            #endregion

            services.AddScoped<RegisterHandler>();
            services.AddScoped<LoginHandler>();
            services.AddScoped<RefreshTokenHandler>();
            return services;
        }
    }
}
