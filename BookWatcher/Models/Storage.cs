﻿using System.Collections.Generic;

namespace BookWatcher.Models
{
    public static class Storage
    {
        // Define your Goodreads key and secret.
        // This can be obtained from https://www.goodreads.com/api/keys.
        public const string ApiKey = "CTtKHTFV48DDQWzqU1FtQ";
        public const string ApiSecret = "J3NTkk9lDmi8jGVXWYC9QTCyDxIWSuDkeWObkS0oY0";

        private static Dictionary<string, string> _tokens = new Dictionary<string, string>();

        public static void SaveToken(string token, string secret)
        {
            if (!_tokens.ContainsKey(token))
                _tokens.Add(token, secret);
        }

        public static string GetSecretToken(string token)
        {
            return _tokens.ContainsKey(token) ? _tokens[token] : null;
        }
    }
}
