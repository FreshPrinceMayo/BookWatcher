using BookWatcher.Models;
using Goodreads;
using Microsoft.AspNetCore.Mvc;
using static BookWatcher.Controllers.HomeController;

namespace BookWatcher.Controllers
{
    public class GoodreadsController : Controller
    {
        public GoodreadsController()
        {

        }
        public IActionResult ConnectGoodReads()
        {
            var client = GoodreadsClient.Create(Storage.ApiKey, Storage.ApiSecret);

            // Ask a Goodreads request token and build an authorization url.
            var callbackUrl = Url.ActionLink("Authenticated", "Home", protocol: "https");
            var requestToken = client.AskCredentials(callbackUrl).Result;

            // Save user token for future used.
            Storage.SaveToken(requestToken.Token, requestToken.Secret);

            // Redirect to Goodreads auth page.
            return Redirect(requestToken.AuthorizeUrl);
        }

        public JsonResult Authenticated(string oauth_token, int authorize)
        {
            var Message = "";


            if (authorize == 0)
            {
                Message = $"Oops, seems you didn't grant an access for the Demo application.";
                return null;
            }

            // Create an unauthorized Goodreads client.
            var client = GoodreadsClient.Create(Storage.ApiKey, Storage.ApiSecret);

            // Get a user's OAuth access token and secret after they have granted access.
            var accessToken = client.GetAccessToken(oauth_token, Storage.GetSecretToken(oauth_token)).Result;

            // Create an authorized Goodreads client.
            var authClient = GoodreadsClient.CreateAuth(Storage.ApiKey, Storage.ApiSecret, accessToken.Token, accessToken.Secret);

            // Get information for the current user.
            var currentUserId = authClient.Users.GetAuthenticatedUserId().Result;
            var user = authClient.Users.GetByUserId(currentUserId).Result;

            var shelveList = authClient.Shelves.GetListOfUserShelves(currentUserId).Result.List;

            foreach (var shelve in shelveList)
            {
                var shelf = shelve.Name;
            }

            var result = authClient.Shelves.AddShelf("Patrick-Collison", false, false, false).Result;


            Message = $"Welcome {user.Name}";

            return new JsonResult(Message);
        }
    }
}