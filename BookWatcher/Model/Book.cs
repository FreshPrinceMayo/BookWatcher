using System;

namespace BookWatcher.Model
{
    public partial class Book
    {
        public int BookId { get; set; }
        public Guid? UserId { get; set; }
        public string Shelf { get; set; }
        public string GoodreadsId { get; set; }
        public string Title { get; set; }
        public string TitleWithoutSeries { get; set; }
        public string Isbn { get; set; }
        public string Isbn13 { get; set; }
        public string RatingsCount { get; set; }
        public string AverageRating { get; set; }
        public string Link { get; set; }
        public string PageCount { get; set; }
        public string Description { get; set; }
        public string Authors { get; set; }
        public string AmazonLink { get; set; }
        public string Asin { get; set; }
        public string ListPrice { get; set; }
        public string CurrentPrice { get; set; }
        public string AffiliateLink { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public virtual User User { get; set; }
    }
}
