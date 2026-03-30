using EcommerceApp.Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace EcommerceApp.Controllers
{
    public class ShopController : Controller
    {
        private readonly IProductService _productService;
        private readonly ICategoryService _categoryService;

        public ShopController(IProductService productService, ICategoryService categoryService)
        {
            _productService = productService;
            _categoryService = categoryService;
        }

        public async Task<IActionResult> Index(int? categoryId)
        {
            var products = await _productService.GetAllAsync();
            if (categoryId.HasValue)
            {
                products = products.Where(p => p.CategoryId == categoryId.Value).ToList();
            }
            
            ViewBag.Categories = await _categoryService.GetAllAsync();
            ViewBag.CurrentCategory = categoryId;
            
            return View(products);
        }

        public async Task<IActionResult> Details(int id)
        {
            var product = await _productService.GetByIdAsync(id);
            if (product == null) return NotFound();
            
            return View(product);
        }
    }
}
