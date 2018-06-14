using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using shortid;
using UrlShortener.Data;

namespace UrlShortener.Web.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ShortenUrl()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ShortenUrl(string originalUrl)
        {
            var repo = new UrlRepository(Properties.Settings.Default.ConStr);
            var url = repo.GetUrl(User.Identity.Name, originalUrl);
            if (url == null)
            {
                var userRepo = new UserRepository(Properties.Settings.Default.ConStr);
                var user = userRepo.GetByEmail(User.Identity.Name);
                var shortId = ShortId.Generate(true, false);
                url = new Url
                {
                    OriginalUrl = originalUrl,
                    ShortenedHash = shortId,
                    UserId = user.Id
                };
                repo.Add(url);
            }

            return Json(new { shortUrl = GetFullUrl(url.ShortenedHash) });
        }

        public ActionResult ViewHistory()
        {
            var repo = new UrlRepository(Properties.Settings.Default.ConStr);
            return View(repo.GetUrlsByEmail(User.Identity.Name));
        }

        private string GetFullUrl(string hash)
        {
            return Request.Url.AbsoluteUri.Replace(Request.Url.PathAndQuery, String.Empty)
                + $"/{hash}";
        }
    }
}