using BookWatcher.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace BookWatcher.Controllers
{
    public class ProfileController : Controller
    {
        public IActionResult Index(Guid id)
        {

            var context = new BookWatcherContext();
            var user = context.User.Include(y => y.Book).FirstOrDefault(x => x.UserId == id);



            var ProfileViewModel = new Models.ProfileViewModel
            {
                Id = id,
                GoodreadsConnected  = user.GoodreadsConnected,
                Books = user.Book
            };

            return View("Index", ProfileViewModel);
        }
    }
}