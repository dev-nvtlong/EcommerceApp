using AutoMapper;
using EcommerceApp.Application.DTOs.Account;
using EcommerceApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EcommerceApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class UserController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMapper _mapper;

        public UserController(UserManager<ApplicationUser> userManager, IMapper mapper)
        {
            _userManager = userManager;
            _mapper = mapper;
        }

        public async Task<IActionResult> Index()
        {
            var users = await _userManager.Users.ToListAsync();
            var userDtos = new List<UserDto>();
            
            foreach (var user in users)
            {
                var dto = _mapper.Map<UserDto>(user);
                dto.Roles = await _userManager.GetRolesAsync(user);
                userDtos.Add(dto);
            }
            
            return View(userDtos);
        }

        public async Task<IActionResult> Details(int id)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());
            if (user == null) return NotFound();
            
            var dto = _mapper.Map<UserDto>(user);
            dto.Roles = await _userManager.GetRolesAsync(user);
            return View(dto);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());
            if (user == null) return NotFound();
            
            var result = await _userManager.DeleteAsync(user);
            if (result.Succeeded)
            {
                TempData["Success"] = "Xóa người dùng thành công!";
            }
            else
            {
                TempData["Error"] = "Không thể xóa người dùng.";
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
