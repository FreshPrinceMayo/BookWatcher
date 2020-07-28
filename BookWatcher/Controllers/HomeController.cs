using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace BookWatcher.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult SignUp(string email)
        {
            return RedirectToAction("Index","Profile");

        }

    }
}
