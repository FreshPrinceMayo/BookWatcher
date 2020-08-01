using BookWatcher.Model;
using BookWatcher.Models;
using Goodreads;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Xml;
using System.Xml.Linq;

namespace BookWatcher.Controllers
{
    public class GoodreadsController : Controller
    {
        public object Link { get; private set; }
        public object Description { get; private set; }

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

        public IActionResult BookList(Guid id)
        {
            var shelf = "read";
            var url = $"https://www.goodreads.com/review/list/80465761.xml?key=CTtKHTFV48DDQWzqU1FtQ&v=2&{shelf}=read&per_page=200&page=1";
            var books = new List<Book>();

            XDocument document = XDocument.Load(url);

            var reviews = document.Root.Elements("reviews").Elements("review").Select(x => new Review
            {
               Id = x?.Element("id")?.Value,
               Title = x?.Element("book")?.Element("title")?.Value,
               TitleWithoutSeries = x?.Element("book")?.Element("title")?.Value,
               ISBN = x?.Element("book")?.Element("isbn")?.Value,
               ISBN13 = x?.Element("book")?.Element("isbn13")?.Value,
               RatingsCount = x?.Element("book")?.Element("ratings_count")?.Value,
               AverageRating = x?.Element("book")?.Element("average_rating")?.Value,
               Link = x?.Element("book")?.Element("link")?.Value,
               PageCount = x?.Element("book")?.Element("num_pages")?.Value,
               Description =  x?.Element("book")?.Element("description")?.Value,
               Authors = x?.Element("book")?.Elements("authors")?.Select(y => y?.Element("author").Element("name")?.Value),
               Shelf = shelf
            }).ToList();


            foreach (var review in reviews)
            {
                books.Add(new Book
                {
                    UserId = id,
                    Shelf = review.Shelf,
                    GoodreadsId = review.Id,
                    Title = review.Title,
                    TitleWithoutSeries = review.TitleWithoutSeries,
                    Isbn = review.ISBN,
                    Isbn13 = review.ISBN13,
                    RatingsCount = review.RatingsCount,
                    AverageRating = review.AverageRating,
                    Link = review.Link,
                    PageCount = review.PageCount,
                    Description = review.Description,
                    Authors = review.Authors.Any() ?  string.Join(",", review.Authors): null
                });
            }

            var context = new BookWatcherContext();

            context.Book.AddRange(books);
            context.SaveChanges();

            return null;
        }

    }
}