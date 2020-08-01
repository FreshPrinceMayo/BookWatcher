using BookWatcher.Model;
using System;
using System.Collections.Generic;

namespace BookWatcher.Models
{
    public class ProfileViewModel
    {
        public Guid Id { get; set; }
        public string Store { get; set; }
        public string Format { get; set; }
        public string Percentage { get; set; }
        public bool GoodreadsConnected { get; set; }
        public int BookCount => 100;
        public string AuthenticationUrl { get; set; }
        public ICollection<Book> Books { get; internal set; }
    }
}
