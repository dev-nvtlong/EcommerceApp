using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using EcommerceApp.Enums;

namespace EcommerceApp.Application.DTOs.Blog
{
    public class CreateBlogPostDto
    {
        [Required(ErrorMessage = "Tiêu đề không được để trống")]
        public string Title { get; set; }

        public string? Content { get; set; }

        public BlogCategory Category { get; set; } = BlogCategory.Experience;
        public string? Tags { get; set; }

        public string? Thumbnail { get; set; }
        public IFormFile? ThumbnailFile { get; set; }
        public List<IFormFile>? ImageFiles { get; set; }

        public bool IsPublished { get; set; } = true;
    }
}
