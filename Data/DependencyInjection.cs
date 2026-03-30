using EcommerceApp.Application.Interfaces.Repositories;
using EcommerceApp.Application.Interfaces.Services;
using EcommerceApp.Application.Services;
using EcommerceApp.Models;
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

            services.AddIdentity<ApplicationUser, IdentityRole<int>>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            #region ============== Đăng ký Repository =====================
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<ICartRepository, CartRepository>();
            #endregion

            #region ============== Đăng ký Service =====================
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<ICartService, CartService>();
            #endregion
            return services;
        }
    }
}
