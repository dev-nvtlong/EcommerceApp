using EcommerceApp.Application.Interfaces.Services;
using EcommerceApp.Enums;
using Microsoft.AspNetCore.Mvc;

namespace EcommerceApp.Controllers
{
    public class ChatController : Controller
    {
        private readonly IProductService _productService;
        private readonly IBlogService _blogService;

        public ChatController(IProductService productService, IBlogService blogService)
        {
            _productService = productService;
            _blogService = blogService;
        }

        [HttpPost]
        public async Task<IActionResult> Query(string message)
        {
            if (string.IsNullOrWhiteSpace(message))
            {
                return Json(new { reply = "Chào bạn! Tôi có thể giúp gì cho bạn hôm nay? Ví dụ: 'Tìm sen đá' hoặc 'Kinh nghiệm trồng cây'." });
            }

            var query = message.ToLower();
            
            // Detailed Intent detection
            var keywordsExperience = new[] { "kinh nghiệm", "cách", "chăm sóc", "mẹo", "hướng dẫn", "trồng", "kỹ thuật", "lưu ý" };
            bool askingAboutExperience = keywordsExperience.Any(k => query.Contains(k));
            
            var results = new List<object>();

            // Always try searching products unless it's strictly an experience question
            if (!askingAboutExperience || query.Length < 10) 
            {
                var products = await _productService.SearchAsync(message);
                foreach (var p in products.Take(3))
                {
                    results.Add(new { 
                        type = "Product", 
                        title = p.Name, 
                        price = p.Price.ToString("N0") + "đ",
                        url = $"/Shop/Details/{p.ProductId}"
                    });
                }
            }

            // Always try searching blogs (tags/content) if it's a general query or specifically about experience
            if (askingAboutExperience || results.Count < 3)
            {
                var posts = await _blogService.GetPublishedPostsAsync(searchTerm: message, category: BlogCategory.Experience);
                // Also search Sales blogs if no Experience blogs found
                if (!posts.Any() && askingAboutExperience) {
                     posts = await _blogService.GetPublishedPostsAsync(searchTerm: message);
                }

                foreach (var p in posts.Take(3))
                {
                    // Avoid duplicates if already found in products (though types are different)
                    if (!results.Any(r => (r as dynamic).title == p.Title)) {
                        results.Add(new { 
                            type = "Blog", 
                            title = p.Title, 
                            author = p.AuthorName,
                            url = $"/Blog/Details/{p.Id}"
                        });
                    }
                }
            }

            if (results.Count == 0)
            {
                return Json(new { reply = "Rất tiếc, tôi chưa tìm thấy kết quả phù hợp với '" + message + "'. Bạn có thể thử từ khóa khác như 'chăm sóc cây' hoặc tên loài cây cụ thể xem sao?" });
            }

            return Json(new { 
                reply = results.Count > 0 ? "Đây là một số gợi ý phù hợp với yêu cầu của bạn:" : "Tôi đã tìm thấy thông tin cho bạn:",
                items = results 
            });
        }
    }
}
