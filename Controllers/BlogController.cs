using Microsoft.AspNetCore.Mvc;

namespace EcommerceApp.Controllers
{
    public class BlogController : Controller
    {
        public IActionResult Index()
        {
            // Placeholder for blog list
            return View();
        }

        public IActionResult Details(int id)
        {
            // Placeholder for blog detail
            return View();
        }
    }
}
