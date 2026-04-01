using EcommerceApp.Application.DTOs.Blog;
using EcommerceApp.Application.Interfaces.Services;
using EcommerceApp.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace EcommerceApp.Controllers
{
    public class BlogController : Controller
    {
        private readonly IBlogService _blogService;

        public BlogController(IBlogService blogService)
        {
            _blogService = blogService;
        }

        private int? UserId => User.Identity?.IsAuthenticated == true 
            ? int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!) 
            : null;

        public async Task<IActionResult> Index(string? searchTerm = null, BlogCategory? category = null)
        {
            var posts = await _blogService.GetPublishedPostsAsync(searchTerm, category);
            ViewData["SearchTerm"] = searchTerm;
            ViewData["Category"] = category;
            return View(posts);
        }

        public async Task<IActionResult> Details(int id)
        {
            var post = await _blogService.GetPostDetailsAsync(id, UserId);
            if (post == null) return NotFound();

            return View(post);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> ToggleLike(int postId)
        {
            if (!UserId.HasValue) return Unauthorized();

            var isLiked = await _blogService.ToggleLikeAsync(postId, UserId.Value);
            return Json(new { success = true, isLiked = isLiked });
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> AddComment(int postId, string content)
        {
            if (!UserId.HasValue) return Unauthorized();
            if (string.IsNullOrWhiteSpace(content)) return Json(new { success = false, message = "Nội dung bình luận không được để trống" });

            try
            {
                var comment = await _blogService.AddCommentAsync(postId, UserId.Value, content);
                return Json(new { success = true, comment = comment });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }
    }
}
