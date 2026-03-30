using AutoMapper;
using EcommerceApp.Application.DTOs.Social;
using EcommerceApp.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EcommerceApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class SocialController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public SocialController(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IActionResult> Comments()
        {
            var comments = await _context.Comments
                .Include(c => c.BlogPost)
                .Include(c => c.User)
                .OrderByDescending(c => c.CreatedAt)
                .ToListAsync();

            var dtos = _mapper.Map<List<CommentDto>>(comments);
            return View(dtos);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteComment(int id)
        {
            var comment = await _context.Comments.FindAsync(id);
            if (comment == null) return Json(new { success = false });

            _context.Comments.Remove(comment);
            await _context.SaveChangesAsync();
            return Json(new { success = true });
        }

        public async Task<IActionResult> Likes()
        {
            var likes = await _context.Likes
                .Include(l => l.BlogPost)
                .Include(l => l.User)
                .ToListAsync();

            var dtos = _mapper.Map<List<LikeDto>>(likes);
            return View(dtos);
        }
    }
}
