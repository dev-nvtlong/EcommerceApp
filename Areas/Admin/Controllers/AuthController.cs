using EcommerceApp.Areas.Admin.ViewModels;
using EcommerceApp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace EcommerceApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AuthController : Controller
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        public AuthController(SignInManager<ApplicationUser> signInManager,
                                UserManager<ApplicationUser> userManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
        }
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = await _userManager.FindByEmailAsync(model.Email);

            if (user == null)
            {
                ModelState.AddModelError("", "Đăng nhập không hợp lệ");
                return View(model);
            }

            var roles = await _userManager.GetRolesAsync(user);
            if (!roles.Contains("Admin"))
            {
                ModelState.AddModelError("", "Bạn không có quyền truy cập");
                return View(model);
            }

            var result = await _signInManager.PasswordSignInAsync(
                user, model.Password, false, false);
            if (result.Succeeded)
            {          
                return RedirectToAction("Index", "Dashboard", new { area = "Admin" });
            }

            ModelState.AddModelError("", "Đăng nhập không hợp lệ");
            return View(model);
        }
    }
}
