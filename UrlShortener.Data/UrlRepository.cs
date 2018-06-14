using System.Linq;
using System.Collections.Generic;

namespace UrlShortener.Data
{
    public class UrlRepository
    {
        private string _connectionString;

        public UrlRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public Url GetUrl(string userEmail, string url)
        {
            using (var context = new UrlShortenerDataContext(_connectionString))
            {
                return context.Urls.FirstOrDefault(u => u.User.EmailAddress == userEmail
                && u.OriginalUrl == url);
            }
        }

        public void Add(Url url)
        {
            using (var context = new UrlShortenerDataContext(_connectionString))
            {
                context.Urls.InsertOnSubmit(url);
                context.SubmitChanges();
            }
        }

        public Url Get(string hash)
        {
            using (var context = new UrlShortenerDataContext(_connectionString))
            {
                return context.Urls.FirstOrDefault(u => u.ShortenedHash.ToLower() == hash.ToLower());
            }
        }

        public void IncrementViews(int urlId)
        {
            using (var context = new UrlShortenerDataContext(_connectionString))
            {
                context.ExecuteCommand("UPDATE Urls SET Views = Views + 1 WHERE Id = {0}", urlId);
            }
        }

        public IEnumerable<Url> GetUrlsByEmail(string email)
        {
            using (var context = new UrlShortenerDataContext(_connectionString))
            {
                return context.Urls.Where(u => u.User.EmailAddress == email).ToList();
            }
        }
    }
}