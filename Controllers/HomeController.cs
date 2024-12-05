using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using DOTNETMVC.Models;
using DOTNETMVC.Models.Tables;  // Ensure the proper namespace is used for Category model

namespace DOTNETMVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        // Changes
        public IActionResult Index()
        {
            using (var db = new Dotnet1BlogDemoContext())
            {
                var indexViewModel = new IndexViewModel(); // Create a new IndexViewModel
                var categories = db.Categories?.ToList() ?? new List<Category>(); // Handle null safely
                indexViewModel.categories = categories; // Assign the categories to the ViewModel
                return View(indexViewModel); // Return the View with the ViewModel
            }
        }
        // Changes end

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

    // Changes
    public class IndexViewModel
    {
        public List<Category>? categories { get; set; } = new List<Category>();
    }
    // Changes end
}