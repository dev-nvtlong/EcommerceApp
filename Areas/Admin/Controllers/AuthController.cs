using BCrypt.Net;
using EcommerceApp.Application.Features.Auth.Login;
using EcommerceApp.Areas.Admin.ViewModels;
using EcommerceApp.Data;
using EcommerceApp.Enums;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace EcommerceApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AuthController : Controller
    {
        private readonly LoginHandler loginHandler;
        public AuthController(LoginHandler loginHandler)
        {
            this.loginHandler = loginHandler;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(
    LoginRequest request,
    CancellationToken cancellationToken)
        {
            var result = await loginHandler.HandleAsync(request, cancellationToken);

            if (!result.Success)
            {
                ModelState.AddModelError("", result.Message);
                return View(request);
            }

            if (!result.Data.Roles.Contains("Admin"))
            {
                ModelState.AddModelError("", "Bạn không có quyền truy cập trang quản trị.");
                return View(request);
            }

            // Login Cookie
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, result.Data.UserId.ToString()),
                new Claim(ClaimTypes.Email, result.Data.Email)
            };

            foreach (var role in result.Data.Roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var identity = new ClaimsIdentity(
                claims,
                CookieAuthenticationDefaults.AuthenticationScheme);

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(identity));

            return RedirectToAction(
                "Index",
                "Dashboard",
                new { area = "Admin" });
        }
    }
}
