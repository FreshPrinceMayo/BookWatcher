using System;
using System.Collections.Generic;

namespace BookWatcher.Model
{
    public partial class AccessToken
    {
        public int AccessTokenId { get; set; }
        public Guid UserId { get; set; }
        public string Token { get; set; }
        public string Secret { get; set; }
        public DateTime? CreatedDate { get; set; }

        public virtual User User { get; set; }
    }
}
