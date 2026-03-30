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
        private readonly AutoMapper.IMapper _mapper;
        private readonly IWebHostEnvironment _hostEnvironment;

        public AccountController(
            UserManager<ApplicationUser> userManager, 
            SignInManager<ApplicationUser> signInManager,
            RoleManager<IdentityRole<int>> roleManager,
            AutoMapper.IMapper mapper,
            IWebHostEnvironment hostEnvironment)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _mapper = mapper;
            _hostEnvironment = hostEnvironment;
        }

        [HttpGet]
        public async Task<IActionResult> Profile()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return RedirectToAction("Auth");
            
            var model = _mapper.Map<UserDto>(user);
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateProfile(UpdateProfileDto model, IFormFile? avatarFile)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return RedirectToAction("Auth");

            if (ModelState.IsValid)
            {
                if (avatarFile != null)
                {
                    string wwwRootPath = _hostEnvironment.WebRootPath;
                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(avatarFile.FileName);
                    string avatarPath = Path.Combine(wwwRootPath, @"uploads\avatars");

                    if (!Directory.Exists(avatarPath))
                    {
                        Directory.CreateDirectory(avatarPath);
                    }

                    // Delete old avatar
                    if (!string.IsNullOrEmpty(user.Avatar))
                    {
                        var oldPath = Path.Combine(wwwRootPath, user.Avatar.TrimStart('/'));
                        if (System.IO.File.Exists(oldPath)) System.IO.File.Delete(oldPath);
                    }

                    using (var fileStream = new FileStream(Path.Combine(avatarPath, fileName), FileMode.Create))
                    {
                        await avatarFile.CopyToAsync(fileStream);
                    }
                    user.Avatar = "/uploads/avatars/" + fileName;
                }

                user.FullName = model.FullName;
                user.PhoneNumber = model.PhoneNumber;
                user.Address = model.Address;
                user.DateOfBirth = model.DateOfBirth;
                user.Gender = model.Gender;

                var result = await _userManager.UpdateAsync(user);
                if (result.Succeeded)
                {
                    TempData["Success"] = "Cập nhật thông tin cá nhân thành công!";
                    return RedirectToAction(nameof(Profile));
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }

            var userDto = _mapper.Map<UserDto>(user);
            return View("Profile", userDto);
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
