using EcommerceApp.Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace EcommerceApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        private readonly IProductService _productService;
        private readonly ICategoryService _categoryService;
        private readonly IWebHostEnvironment _hostEnvironment;

        public ProductController(IProductService productService, ICategoryService categoryService, IWebHostEnvironment hostEnvironment)
        {
            _productService = productService;
            _categoryService = categoryService;
            _hostEnvironment = hostEnvironment;
        }

        public async Task<IActionResult> Index()
        {
            ViewBag.Categories = await _categoryService.GetAllAsync();
            var products = await _productService.GetAllAsync();
            return View(products);
        }

        [HttpGet]
        public async Task<IActionResult> GetById(int id)
        {
            var product = await _productService.GetByIdAsync(id);
            if (product == null) return NotFound();
            return Json(product);
        }

        [HttpPost]
        public async Task<IActionResult> Upsert(EcommerceApp.Application.DTOs.Product.ProductDto dto, IFormFile? ImageFile)
        {
            if (ModelState.IsValid)
            {
                if (ImageFile != null)
                {
                    string wwwRootPath = _hostEnvironment.WebRootPath;
                    
                    // Delete old image if updating
                    if (dto.ProductId != 0)
                    {
                        var productFromDb = await _productService.GetByIdAsNoTrackingAsync(dto.ProductId);
                        if (productFromDb != null && productFromDb.ImageUrls != null && productFromDb.ImageUrls.Any())
                        {
                            var oldImagePath = Path.Combine(wwwRootPath, productFromDb.ImageUrls[0].TrimStart('/'));
                            if (System.IO.File.Exists(oldImagePath))
                            {
                                System.IO.File.Delete(oldImagePath);
                            }
                        }
                    }

                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(ImageFile.FileName);
                    string productPath = Path.Combine(wwwRootPath, @"uploads\products");

                    if (!Directory.Exists(productPath))
                    {
                        Directory.CreateDirectory(productPath);
                    }

                    using (var fileStream = new FileStream(Path.Combine(productPath, fileName), FileMode.Create))
                    {
                        await ImageFile.CopyToAsync(fileStream);
                    }

                    dto.ImageUrls = new List<string> { "/uploads/products/" + fileName };
                }

                if (dto.ProductId == 0)
                {
                    dto.IsActive = true;
                    var newId = await _productService.CreateAsync(dto);
                    TempData["Success"] = "Thêm sản phẩm thành công!";
                    TempData["OpenImportModal"] = newId; // Trigger modal for new product
                }
                else
                {
                    await _productService.UpdateAsync(dto);
                    TempData["Success"] = "Cập nhật sản phẩm thành công!";
                }
                return RedirectToAction(nameof(Index));
            }
            ViewBag.Categories = await _categoryService.GetAllAsync();
            return View(nameof(Index), await _productService.GetAllAsync());
        }

        [HttpPost]
        public async Task<IActionResult> ImportStock(int productId, int quantity, decimal costPrice)
        {
            try
            {
                await _productService.ImportStockAsync(productId, quantity, costPrice);
                return Json(new { success = true, message = "Nhập hàng thành công!" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> ToggleActive(int id)
        {
            try
            {
                var product = await _productService.GetByIdAsync(id);
                if (product == null) return Json(new { success = false, message = "Không tìm thấy sản phẩm" });

                product.IsActive = !product.IsActive;
                await _productService.UpdateAsync(product);
                return Json(new { success = true, isActive = product.IsActive });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            // Delete image file before deleting product
            var product = await _productService.GetByIdAsync(id);
            if (product != null && product.ImageUrls != null && product.ImageUrls.Any())
            {
                var imagePath = Path.Combine(_hostEnvironment.WebRootPath, product.ImageUrls[0].TrimStart('/'));
                if (System.IO.File.Exists(imagePath))
                {
                    System.IO.File.Delete(imagePath);
                }
            }

            await _productService.DeleteAsync(id);
            TempData["Success"] = "Xóa sản phẩm thành công!";
            return RedirectToAction(nameof(Index));
        }
    }
}
