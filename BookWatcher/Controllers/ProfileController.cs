using BookWatcher.Model;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;

namespace BookWatcher.Controllers
{
    public class ProfileController : Controller
    {
        public IActionResult Index(Guid id)
        {

            var context = new BookWatcherContext();
            var user = context.User.FirstOrDefault(x => x.UserId == id);



            var ProfileViewModel = new Models.ProfileViewModel
            {
                Id = id,
                Store = "",
                Format = "",
                Percentage = "",
                AuthenticationUrl = "", 
                GoodreadsConnected  = user.GoodreadsConnected
            };

            return View("Index", ProfileViewModel);
        }
    }
}