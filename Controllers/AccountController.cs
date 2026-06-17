using EcommerceApp.Application.DTOs.Account;
using EcommerceApp.Application.Features.Auth.Login;
using EcommerceApp.Application.Features.Auth.Register;
using EcommerceApp.Application.Interfaces.Services;
using EcommerceApp.Models.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace EcommerceApp.Controllers
{
    public class AccountController : Controller
    {
        private readonly LoginHandler _loginHandler;
        private readonly RegisterHandler _registerHandler;
        private readonly IUserService _userService;

        public AccountController(
            LoginHandler loginHandler,
            RegisterHandler registerHandler,
            IUserService userService)
        {
            _loginHandler = loginHandler;
            _registerHandler = registerHandler;
            _userService = userService;
        }



        //// POST: Đăng nhập từ trang Auth
        //[HttpPost]
        //[ActionName("Login")]
        //public async Task<IActionResult> AuthLogin(
        //    AuthViewModel vm,
        //    CancellationToken cancellationToken)
        //{
        //    ModelState.Clear();
        //    if (!TryValidateModel(vm.LoginModel, nameof(vm.LoginModel)))
        //    {
        //        vm.IsRegisterActive = false;
        //        return View("Auth", vm);
        //    }

        //    var request = new LoginRequest
        //    {
        //        Email = vm.LoginModel.Email,
        //        Password = vm.LoginModel.Password
        //    };

        //    var result = await _loginHandler.HandleAsync(request, cancellationToken);

        //    if (!result.Success)
        //    {
        //        ModelState.AddModelError("", result.Message ?? "Đăng nhập thất bại.");
        //        vm.IsRegisterActive = false;
        //        return View("Auth", vm);
        //    }

        //    var claims = new List<Claim>
        //    {
        //        new Claim(ClaimTypes.NameIdentifier, result.Data.UserId.ToString()),
        //        new Claim(ClaimTypes.Email, result.Data.Email),
        //    };

        //    foreach (var role in result.Data.Roles)
        //        claims.Add(new Claim(ClaimTypes.Role, role));

        //    var claimsIdentity = new ClaimsIdentity(
        //        claims, CookieAuthenticationDefaults.AuthenticationScheme);

        //    var authProperties = new AuthenticationProperties
        //    {
        //        IsPersistent = vm.LoginModel.RememberMe,
        //        ExpiresUtc = DateTimeOffset.UtcNow.AddDays(7)
        //    };

        //    await HttpContext.SignInAsync(
        //        CookieAuthenticationDefaults.AuthenticationScheme,
        //        new ClaimsPrincipal(claimsIdentity), authProperties);

        //    return RedirectToAction("Index", "Home");
        //}

        // GET: Đăng nhập
        [HttpGet]
        public IActionResult Login(string? returnUrl = null)
        {
            if (User.Identity?.IsAuthenticated == true)
                return RedirectToAction("Index", "Home");

            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginRequest loginRequest, string? returnUrl, CancellationToken cancellationToken)
        {
            ViewData["ReturnUrl"] = returnUrl;

            if (!ModelState.IsValid)
                return View(loginRequest);

            var result = await _userService.LoginAsync(loginRequest, cancellationToken);

            if (!result.Success || result.Data == null)
            {
                ModelState.AddModelError("", result.Message ?? "Đăng nhập thất bại");
                return View(loginRequest);
            }

            Response.Cookies.Append(
                "access_token",
                result.Data.AccessToken,
                new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    Expires = DateTimeOffset.UtcNow.AddDays(1)
                });

            Response.Cookies.Append(
                "refresh_token",
                result.Data.RefreshToken,
                new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    Expires = DateTimeOffset.UtcNow.AddDays(30)
                });

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, result.Data.UserId.ToString()),
                new Claim(ClaimTypes.Email, result.Data.Email),
            };

            if (result.Data.Roles != null)
            {
                foreach (var role in result.Data.Roles)
                    claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var claimsIdentity = new ClaimsIdentity(
                claims, CookieAuthenticationDefaults.AuthenticationScheme);

            var authProperties = new AuthenticationProperties
            {
                IsPersistent = true,
                ExpiresUtc = DateTimeOffset.UtcNow.AddDays(7)
            };

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity), authProperties);

            if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                return LocalRedirect(returnUrl);

            return RedirectToAction("Index", "Home");
        }

        // GET: Đăng ký
        [HttpGet]
        public IActionResult Register()
        {
            if (User.Identity?.IsAuthenticated == true)
                return RedirectToAction("Index", "Home");

            return View();
        }

        // POST: Đăng ký
        [HttpPost]
        public async Task<IActionResult> Register(
            RegisterRequest request,
            CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
            {
                return View(request);
            }

            var result = await _registerHandler.HandleAsync(request, cancellationToken);

            if (!result.Success)
            {
                ModelState.AddModelError("", result.Message ?? "Đăng ký thất bại.");
                return View(request);
            }

            TempData["RegisterSuccess"] = "Đăng ký thành công! Vui lòng đăng nhập.";
            return RedirectToAction("Login");
        }

        // POST: Đăng xuất
        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }

        // GET: Trang hồ sơ cá nhân
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Profile()
        {
            var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!Guid.TryParse(userIdStr, out Guid userId))
                return RedirectToAction("Login");

            var userDto = await _userService.GetProfileAsync(userId);
            if (userDto == null)
                return RedirectToAction("Login");

            return View(userDto);
        }

        // POST: Cập nhật thông tin cá nhân
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> UpdateProfile(
            UpdateProfileDto model,
            IFormFile? avatarFile)
        {
            var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (!Guid.TryParse(userIdStr, out Guid userId))
                return RedirectToAction("Login");

            await _userService.UpdateProfileAsync(userId, model);

            return RedirectToAction("Profile");
        }
        // ========= Update avatar riêng biệt nếu muốn =========
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> UpdateAvatar(IFormFile avatarFile)
        {
            var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (!Guid.TryParse(userIdStr, out Guid userId))
                return RedirectToAction("Login");

            await _userService.UpdateAvatarAsync(userId, avatarFile);

            return RedirectToAction("Profile");
        }
    }
}
