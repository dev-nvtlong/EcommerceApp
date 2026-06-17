using EcommerceApp.Application.Interfaces.Services;
using EcommerceApp.Models;
using EcommerceApp.Data;
using Microsoft.AspNetCore.Mvc;

namespace EcommerceApp.Controllers
{
    public class ShopController : Controller
    {
        private readonly IProductService _productService;
        private readonly ICategoryService _categoryService;
        private readonly IReviewService _reviewService;
        private readonly ApplicationDbContext _context;

        public ShopController(IProductService productService, ICategoryService categoryService, IReviewService reviewService, ApplicationDbContext context)
        {
            _productService = productService;
            _categoryService = categoryService;
            _reviewService = reviewService;
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> SubmitReview(Guid productId, int rating, string comment)
        {
            var userIdStr = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdStr)) return Json(new { success = false, message = "Bạn cần đăng nhập để đánh giá" });

            var userId = Guid.Parse(userIdStr);
            var result = await _reviewService.AddReviewAsync(userId, productId, rating, comment);
            return Json(new { success = result });
        }

        public async Task<IActionResult> Index(Guid? categoryId, decimal? minPrice, decimal? maxPrice, string? searchTerm, string? sortOrder = "newest")
        {
            var products = await _productService.GetAllActiveAsync();
            
            // Search by Name
            if (!string.IsNullOrEmpty(searchTerm))
            {
                products = products.Where(p => p.Name.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)).ToList();
            }

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

            // Combine Sorting with Stock Status (Stock > 0 first)
            var query = products.OrderByDescending(p => p.StockQuantity > 0);

            products = sortOrder switch
            {
                "price_asc" => query.ThenBy(p => p.Price).ToList(),
                "price_desc" => query.ThenByDescending(p => p.Price).ToList(),
                "newest" => query.ThenByDescending(p => p.ProductId).ToList(),
                _ => query.ThenByDescending(p => p.ProductId).ToList()
            };
            
            ViewBag.Categories = await _categoryService.GetAllAsync();
            ViewBag.CurrentCategory = categoryId;
            ViewBag.MinPrice = minPrice;
            ViewBag.MaxPrice = maxPrice;
            ViewBag.SearchTerm = searchTerm;
            ViewBag.SortOrder = sortOrder;
            
            return View(products);
        }

        public async Task<IActionResult> Details(Guid id)
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

            //// User Info for Pre-filling Shipping (Without modifying profile)
            //if (User.Identity?.IsAuthenticated == true)
            //{
            //    var userIdStr = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            //    if (Guid.TryParse(userIdStr, out Guid userId))
            //    {
            //        var user = await _context.Users.FindAsync(userId);
            //        if (user != null)
            //        {
            //            ViewBag.UserAddress = user.Address;
            //        }
            //    }
            //}
                
            return View(product);
        }
    }
}
