using Microsoft.AspNetCore.Mvc;
using System;

namespace BookWatcher.Controllers
{
    public class ProfileController : Controller
    {
        public IActionResult Index(Guid id)
        {
            var ProfileViewModel = new Models.ProfileViewModel
            {
                Id = id,
                Store = "",
                Format = "",
                Percentage = "",
                AuthenticationUrl = ""
            };

            return View("Index", ProfileViewModel);
        }
    }
}