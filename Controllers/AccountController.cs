using EcommerceApp.Application.DTOs.Account;
using EcommerceApp.Models;
using EcommerceApp.Models.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace EcommerceApp.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<IdentityRole<int>> _roleManager;

        public AccountController(
            UserManager<ApplicationUser> userManager, 
            SignInManager<ApplicationUser> signInManager,
            RoleManager<IdentityRole<int>> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }

        [HttpGet]
        public IActionResult Auth(string mode = "login")
        {
            if (User.Identity?.IsAuthenticated == true) return RedirectToAction("Index", "Home");
            var model = new AuthViewModel { IsRegisterActive = mode == "register" };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Register(AuthViewModel model)
        {
            // Chỉ validate RegisterModel khi post Register
            var registerErrors = ModelState.Where(x => x.Key.StartsWith("RegisterModel")).SelectMany(x => x.Value.Errors);
            if (registerErrors.Any())
            {
                model.IsRegisterActive = true;
                return View("Auth", model);
            }

            var user = new ApplicationUser
            {
                UserName = model.RegisterModel.Email,
                Email = model.RegisterModel.Email,
                FullName = model.RegisterModel.FullName
            };

            var result = await _userManager.CreateAsync(user, model.RegisterModel.Password);

            if (result.Succeeded)
            {
                // Gán role Customer
                if (!await _roleManager.RoleExistsAsync("Customer"))
                {
                    await _roleManager.CreateAsync(new IdentityRole<int>("Customer"));
                }
                await _userManager.AddToRoleAsync(user, "Customer");

                await _signInManager.SignInAsync(user, isPersistent: false);
                return RedirectToAction("Index", "Home");
            }

            foreach (var error in result.Errors)
            {
                if (error.Code.Contains("Email"))
                    ModelState.AddModelError("RegisterModel.Email", error.Description);
                else if (error.Code.Contains("Password"))
                    ModelState.AddModelError("RegisterModel.Password", error.Description);
                else
                    ModelState.AddModelError("RegisterModel", error.Description);
            }

            model.IsRegisterActive = true;
            return View("Auth", model);
        }

        [HttpPost]
        public async Task<IActionResult> Login(AuthViewModel model)
        {
            // Chỉ validate LoginModel khi post Login
            var loginErrors = ModelState.Where(x => x.Key.StartsWith("LoginModel")).SelectMany(x => x.Value.Errors);
            if (loginErrors.Any())
            {
                model.IsRegisterActive = false;
                return View("Auth", model);
            }

            var result = await _signInManager.PasswordSignInAsync(model.LoginModel.Email, model.LoginModel.Password, model.LoginModel.RememberMe, lockoutOnFailure: false);

            if (result.Succeeded)
            {
                return RedirectToAction("Index", "Home");
            }

            ModelState.AddModelError("LoginModel", "Email hoặc mật khẩu không chính xác.");
            model.IsRegisterActive = false;
            return View("Auth", model);
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}
