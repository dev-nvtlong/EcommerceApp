using EcommerceApp.Application.DTOs.Blog;
using EcommerceApp.Application.Interfaces.Services;
using EcommerceApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace EcommerceApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class BlogController : Controller
    {
        private readonly IBlogService _blogService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public BlogController(IBlogService blogService, UserManager<ApplicationUser> userManager, IWebHostEnvironment webHostEnvironment)
        {
            _blogService = blogService;
            _userManager = userManager;
            _webHostEnvironment = webHostEnvironment;
        }

        private int UserId => int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        public async Task<IActionResult> Index()
        {
            var posts = await _blogService.GetAllPostsAsync();
            return View(posts);
        }

        public IActionResult Create()
        {
            return View(new CreateBlogPostDto());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateBlogPostDto dto)
        {
            if (!ModelState.IsValid) return View(dto);

            if (dto.ThumbnailFile != null)
            {
                dto.Thumbnail = await SaveThumbnail(dto.ThumbnailFile);
            }

            await _blogService.CreatePostAsync(UserId, dto);
            TempData["Success"] = "Thêm bài viết mới thành công!";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var post = await _blogService.GetPostDetailsAsync(id);
            if (post == null) return NotFound();

            var dto = new CreateBlogPostDto
            {
                Title = post.Title,
                Content = post.Content,
                Thumbnail = post.Thumbnail,
                IsPublished = post.IsPublished
            };
            return View("Create", dto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, CreateBlogPostDto dto)
        {
            if (!ModelState.IsValid) return View("Create", dto);

            if (dto.ThumbnailFile != null)
            {
                dto.Thumbnail = await SaveThumbnail(dto.ThumbnailFile);
            }

            var result = await _blogService.UpdatePostAsync(id, dto);
            if (!result) return NotFound();

            TempData["Success"] = "Cập nhật bài viết thành công!";
            return RedirectToAction(nameof(Index));
        }

        private async Task<string> SaveThumbnail(IFormFile file)
        {
            var uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "uploads", "blogs");
            if (!Directory.Exists(uploadsFolder)) Directory.CreateDirectory(uploadsFolder);

            var uniqueFileName = Guid.NewGuid().ToString() + "_" + file.FileName;
            var filePath = Path.Combine(uploadsFolder, uniqueFileName);

            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(fileStream);
            }

            return "/uploads/blogs/" + uniqueFileName;
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _blogService.DeletePostAsync(id);
            if (!result) return Json(new { success = false });

            return Json(new { success = true });
        }
    }
}
