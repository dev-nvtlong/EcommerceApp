using System.Diagnostics;
using EcommerceApp.Application.Interfaces.Services;
using EcommerceApp.Enums;
using EcommerceApp.Models;
using Microsoft.AspNetCore.Mvc;

namespace EcommerceApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IProductService _productService;
        private readonly ICategoryService _categoryService;
        private readonly IBlogService _blogService;

        public HomeController(ILogger<HomeController> logger, 
                            IProductService productService, 
                            ICategoryService categoryService,
                            IBlogService blogService)
        {
            _logger = logger;
            _productService = productService;
            _categoryService = categoryService;
            _blogService = blogService;
        }

        public async Task<IActionResult> Index()
        {
            var categories = await _categoryService.GetAllAsync();
            var products = await _productService.GetAllActiveAsync();
            var blogPosts = await _blogService.GetPublishedPostsAsync(category: BlogCategory.Sales);
            
            ViewBag.Categories = categories.Take(4).ToList();
            ViewBag.FeaturedProducts = products.Where(p => p.IsFeatured).Take(8).ToList();
            ViewBag.LatestSalesPosts = blogPosts.Take(4).ToList();
            
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
