using EcommerceApp.Application.DTOs.Blog;
using EcommerceApp.Application.Interfaces.Services;
using EcommerceApp.Models;
using EcommerceApp.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace EcommerceApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class BlogController : Controller
    {
        private readonly IBlogService _blogService;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public BlogController(IBlogService blogService, IWebHostEnvironment webHostEnvironment)
        {
            _blogService = blogService;
            _webHostEnvironment = webHostEnvironment;
        }

        private Guid UserId => Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

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
            if (!ModelState.IsValid) return Json(new { success = false, message = "Dữ liệu không hợp lệ" });

            if (dto.ThumbnailFile != null)
            {
                dto.Thumbnail = await SaveFile(dto.ThumbnailFile);
            }

            var postDto = await _blogService.CreatePostAsync(UserId, dto);
            
            // Save additional images
            if (dto.ImageFiles != null && dto.ImageFiles.Any())
            {
                var imageUrls = new List<string>();
                foreach (var file in dto.ImageFiles)
                {
                    imageUrls.Add(await SaveFile(file));
                }
                await _blogService.AddPostImagesAsync(postDto.Id, imageUrls);
            }

            return Json(new { success = true, id = postDto.Id });
        }

        public async Task<IActionResult> Edit(Guid id)
        {
            var post = await _blogService.GetPostDetailsAsync(id);
            if (post == null) return NotFound();

            return Json(post);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, CreateBlogPostDto dto)
        {
            if (!ModelState.IsValid) return Json(new { success = false, message = "Dữ liệu không hợp lệ" });

            if (dto.ThumbnailFile != null)
            {
                dto.Thumbnail = await SaveFile(dto.ThumbnailFile);
            }

            var result = await _blogService.UpdatePostAsync(id, dto);
            if (!result) return Json(new { success = false, message = "Không tìm thấy bài viết" });

            // Save additional images (append)
            if (dto.ImageFiles != null && dto.ImageFiles.Any())
            {
                var imageUrls = new List<string>();
                foreach (var file in dto.ImageFiles)
                {
                    imageUrls.Add(await SaveFile(file));
                }
                await _blogService.AddPostImagesAsync(id, imageUrls);
            }

            return Json(new { success = true, message = "Cập nhật bài viết thành công!" });
        }

        private async Task<string> SaveFile(IFormFile file)
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
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _blogService.DeletePostAsync(id);
            if (!result) return Json(new { success = false });

            return Json(new { success = true });
        }

        [HttpPost]
        public async Task<IActionResult> UploadImage(IFormFile upload)
        {
            if (upload == null || upload.Length == 0) return Json(new { error = new { message = "Lỗi tải ảnh" } });

            var url = await SaveFile(upload);
            return Json(new { url });
        }
    }
}
