using BookWatcher.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;

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
            var context = new BookWatcherContext();
            var userId = Guid.NewGuid();

            context.User.Add(new User
            {
                Email = email,
                UserId = userId
            });
            context.SaveChanges();

            return RedirectToAction("Index","Profile", new { id = userId });

        }

    }
}
