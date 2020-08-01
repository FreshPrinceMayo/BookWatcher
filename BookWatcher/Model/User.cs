using System;
using System.Collections.Generic;

namespace BookWatcher.Model
{
    public partial class User
    {
        public User()
        {
            AccessToken = new HashSet<AccessToken>();
            Book = new HashSet<Book>();
        }

        public Guid UserId { get; set; }
        public string Email { get; set; }
        public bool GoodreadsConnected { get; set; }
        public long? GoodreadsUserId { get; set; }

        public virtual ICollection<AccessToken> AccessToken { get; set; }
        public virtual ICollection<Book> Book { get; set; }
    }
}
