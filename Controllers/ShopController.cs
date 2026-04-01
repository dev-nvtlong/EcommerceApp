using System.Linq;
using EcommerceApp.Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace EcommerceApp.Controllers
{
    public class ShopController : Controller
    {
        private readonly IProductService _productService;
        private readonly ICategoryService _categoryService;
        private readonly IReviewService _reviewService;

        public ShopController(IProductService productService, ICategoryService categoryService, IReviewService reviewService)
        {
            _productService = productService;
            _categoryService = categoryService;
            _reviewService = reviewService;
        }

        [HttpPost]
        public async Task<IActionResult> SubmitReview(int productId, int rating, string comment)
        {
            var userIdStr = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdStr)) return Json(new { success = false, message = "Bạn cần đăng nhập để đánh giá" });

            var result = await _reviewService.AddReviewAsync(int.Parse(userIdStr), productId, rating, comment);
            return Json(new { success = result });
        }

        public async Task<IActionResult> Index(int? categoryId, decimal? minPrice, decimal? maxPrice, string? sortOrder = "newest")
        {
            var products = await _productService.GetAllActiveAsync();
            
            // Filter by Category
            if (categoryId.HasValue)
            {
                products = products.Where(p => p.CategoryId == categoryId.Value).ToList();
            }

            // Filter by Price
            if (minPrice.HasValue)
            {
                products = products.Where(p => p.Price >= minPrice.Value).ToList();
            }
            if (maxPrice.HasValue)
            {
                products = products.Where(p => p.Price <= maxPrice.Value).ToList();
            }

            // Sorting
            products = sortOrder switch
            {
                "price_asc" => products.OrderBy(p => p.Price).ToList(),
                "price_desc" => products.OrderByDescending(p => p.Price).ToList(),
                "newest" => products.OrderByDescending(p => p.ProductId).ToList(),
                _ => products.ToList()
            };
            
            ViewBag.Categories = await _categoryService.GetAllAsync();
            ViewBag.CurrentCategory = categoryId;
            ViewBag.MinPrice = minPrice;
            ViewBag.MaxPrice = maxPrice;
            ViewBag.SortOrder = sortOrder;
            
            return View(products);
        }

        public async Task<IActionResult> Details(int id)
        {
            var product = await _productService.GetByIdAsync(id);
            if (product == null || !product.IsActive) return NotFound();

            var reviews = await _reviewService.GetByProductIdAsync(id);
            ViewBag.Reviews = reviews;
            ViewBag.ReviewCount = reviews.Count;
            ViewBag.AverageRating = reviews.Any() ? (double)reviews.Average(r => r.Rating) : 0;
            
            // Related Products
            var allProducts = await _productService.GetAllActiveAsync();
            ViewBag.RelatedProducts = allProducts
                .Where(p => p.CategoryId == product.CategoryId && p.ProductId != id)
                .Take(4)
                .ToList();
                
            return View(product);
        }
    }
}
