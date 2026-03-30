using EcommerceApp.Data;
using EcommerceApp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

#region ================== SERVICES ==================
builder.Services.AddControllersWithViews();

// Session
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

// DbContext + Identity (bạn phải có AddInfrastructure)
builder.Services.AddInfrastructure(builder.Configuration);

// AutoMapper
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

// Cookie
builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Account/Auth";
    options.AccessDeniedPath = "/Account/AccessDenied";
    options.LogoutPath = "/Account/Auth";

    options.Cookie.Name = "CayCanhAuth";
});

#endregion
var app = builder.Build();
#region ================== SEED DATA ==================

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
    var roleManager = services.GetRequiredService<RoleManager<IdentityRole<int>>>();

    await ApplicationDbContext.SeedAsync(userManager, roleManager);
}

#endregion

#region ================== MIDDLEWARE ==================

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}


app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseSession();

// QUAN TR?NG: Authentication phải trước Authorization
app.UseAuthentication();
app.UseAuthorization();

#endregion

#region ================== ROUTING ==================

// Route cho Area (Admin)
app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Dashboard}/{action=Index}/{id?}");

// Route mặc định (Customer)
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

#endregion


app.Run();
