using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookWatcher.Model
{
    public class Review
    {
        public string Id { get; set; }
        public string Title { get; internal set; }
        public string Rating { get; internal set; }
        public string TitleWithoutSeries { get; internal set; }
        public string ISBN { get; internal set; }
        public string ISBN13 { get; internal set; }
        public object TextReviewCount { get; internal set; }
        public string AverageRating { get; internal set; }
        public string Link { get; internal set; }
        public string Description { get; internal set; }
        public string RatingsCount { get; internal set; }
        public IEnumerable<string> Authors { get; internal set; }
        public string PageCount { get; internal set; }
        public string Shelf { get; internal set; }
    }
}
