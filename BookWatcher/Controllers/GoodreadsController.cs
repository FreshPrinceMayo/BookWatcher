using BookWatcher.Model;
using BookWatcher.Models;
using Goodreads;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Data.Entity;
using System.Linq;

namespace BookWatcher.Controllers
{
    public class GoodreadsController : Controller
    {
        public GoodreadsController()
        {

        }
        public IActionResult ConnectGoodReads(Guid id)
        {
            var client = GoodreadsClient.Create(Storage.ApiKey, Storage.ApiSecret);

            // Ask a Goodreads request token and build an authorization url.
            var callbackUrl = Url.ActionLink("Authenticated", "Goodreads");
            var requestToken = client.AskCredentials(callbackUrl).Result;

            // Save user token for future used.
            new Storage().SaveToken(id,requestToken.Token, requestToken.Secret);

            // Redirect to Goodreads auth page.
            return Redirect(requestToken.AuthorizeUrl);
        }

        public JsonResult Check(Guid userId)
        {
            var client = GoodreadsClient.Create(Storage.ApiKey, Storage.ApiSecret);
            var accessToken = client.GetAccessToken(new Storage().GetSecret(userId), new Storage().GetSecret(userId)).Result;
            var authClient = GoodreadsClient.CreateAuth(Storage.ApiKey, Storage.ApiSecret, accessToken.Token, accessToken.Secret);

            var currentUserId = authClient.Users.GetAuthenticatedUserId().Result;
            var user = authClient.Users.GetByUserId(currentUserId).Result;
            var message = $"Welcome {user.Name}";

            return new JsonResult(message);
        }

        public IActionResult Authenticated(string oauth_token, int authorize)
        {
            var context = new BookWatcherContext();

            var accessToken = context.AccessToken.FirstOrDefault(x => x.Token == oauth_token);
            var user = context.User.FirstOrDefault(x => x.UserId == accessToken.UserId);

            if (authorize == 1)
            {
                user.GoodreadsConnected = true;
                context.User.Update(user);
                context.SaveChanges();
            }

            if (authorize == 0)
            {
                var message = $"Oops, seems you didn't grant an access for the Demo application.";
                return null;
            }

            return RedirectToAction("Index", "Profile", new { id  = user.UserId});
        }

        public JsonResult SaveGoodReadsUserId(Guid id)
        {
            try
            {
                var client = GoodreadsClient.Create(Storage.ApiKey, Storage.ApiSecret);
                var accessToken = client.GetAccessToken(new Storage().GetToken(id), new Storage().GetSecret(id)).Result;
                var authClient = GoodreadsClient.CreateAuth(Storage.ApiKey, Storage.ApiSecret, accessToken.Token, accessToken.Secret);
                var currentUserId = authClient.Users.GetAuthenticatedUserId().Result;

                if(currentUserId != null)
                {
                    var context = new BookWatcherContext();
                    var user = context.User.FirstOrDefault(x => x.UserId == id);
                    user.GoodreadsUserId = currentUserId;
                    context.User.Update(user);
                    context.SaveChanges();
                }

                return new JsonResult($"True");

            }
            catch (Exception ex)
            {
                throw ex;
            }





        }


        //public JsonResult Sync(Guid id)
        //{

        //    var client = GoodreadsClient.Create(Storage.ApiKey, Storage.ApiSecret);

        //    // Get a user's OAuth access token and secret after they have granted access.
        //    var accessToken = client.GetAccessToken(new Storage().GetToken(id), new Storage().GetSecret(id)).Result;

        //    // Create an authorized Goodreads client.
        //    var authClient = GoodreadsClient.CreateAuth(Storage.ApiKey, Storage.ApiSecret, accessToken.Token, accessToken.Secret);

        //    // Get information for the current user.
        //    var currentUserId =  authClient.Users.GetAuthenticatedUserId().Result;

           
             
        //    var user = authClient.Users.GetByUserId(currentUserId).Result;
        //    user.b

        //    var bookCount = user.Shelves.FirstOrDefault().BookCount;
        //    var favouriteBooks = user.FavoriteBooks;

        //    return new JsonResult($"Book Count {bookCount}");
        //}

    }
}