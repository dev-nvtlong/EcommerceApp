using EcommerceApp.Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace EcommerceApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoryController : Controller
    {
        private readonly ICategoryService _categoryService;
        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }
        public async Task<IActionResult> Index()
        {
            var categories = await _categoryService.GetAllAsync();
            return View(categories);
        }

        [HttpGet]
        public async Task<IActionResult> GetById(int id)
        {
            var category = await _categoryService.GetByIdAsync(id);
            if (category == null) return NotFound();
            return Json(category);
        }

        [HttpPost]
        public async Task<IActionResult> Upsert(EcommerceApp.Application.DTOs.Category.CategoryDto dto)
        {
            if (ModelState.IsValid)
            {
                if (dto.Id == 0)
                {
                    await _categoryService.CreateAsync(dto);
                    TempData["Success"] = "Thêm danh mục thành công!";
                }
                else
                {
                    await _categoryService.UpdateAsync(dto);
                    TempData["Success"] = "Cập nhật danh mục thành công!";
                }
                return RedirectToAction(nameof(Index));
            }
            return View(nameof(Index), await _categoryService.GetAllAsync());
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            await _categoryService.DeleteAsync(id);
            TempData["Success"] = "Xóa danh mục thành công!";
            return RedirectToAction(nameof(Index));
        }
    }
}
