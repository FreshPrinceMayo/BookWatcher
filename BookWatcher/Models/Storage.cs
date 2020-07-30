using BookWatcher.Model;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

namespace BookWatcher.Models
{
    public class Storage
    {
        private BookWatcherContext _dbContext;
        public Storage()
        {
            _dbContext = new BookWatcherContext();
        }

        // Define your Goodreads key and secret.
        // This can be obtained from https://www.goodreads.com/api/keys.
        public const string ApiKey = "CTtKHTFV48DDQWzqU1FtQ";
        public const string ApiSecret = "J3NTkk9lDmi8jGVXWYC9QTCyDxIWSuDkeWObkS0oY0";
        public void SaveToken(Guid id,string token, string secret)
        {
            _dbContext.AccessToken.Add(new AccessToken
            {
                UserId = id,
                Secret = secret,
                Token = token,
                CreatedDate = DateTime.Now
            });

            _dbContext.SaveChanges();
        }
       
        public string GetSecret(Guid userId)
        {
            return _dbContext.AccessToken.OrderByDescending(y => y.CreatedDate).FirstOrDefault(x => x.UserId == userId).Secret ?? null;
        }

        public string GetToken(Guid userId)
        {
            return _dbContext.AccessToken.OrderByDescending(y => y.CreatedDate).FirstOrDefault(x => x.UserId == userId).Token ?? null;
        }
    }
}
