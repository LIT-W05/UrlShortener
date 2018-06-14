using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using UrlShortener.Data;

namespace UrlShortener.Web.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(string email, string password)
        {
            var repo = new UserRepository(Properties.Settings.Default.ConStr);
            var user = repo.Login(email, password);
            if (user == null)
            {
                return RedirectToAction("Login");
            }

            FormsAuthentication.SetAuthCookie(user.EmailAddress, false);
            return RedirectToAction("Index", "Account");
        }

        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Register(User user, string password)
        {
            var repo = new UserRepository(Properties.Settings.Default.ConStr);
            repo.AddUser(user, password);
            return RedirectToAction("Login");
        }

        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login");
        }

        [Route("{urlhash}")]
        public ActionResult ViewShortenedUrl(string urlHash)
        {
            var urlRepo = new UrlRepository(Properties.Settings.Default.ConStr);
            var url = urlRepo.Get(urlHash);
            if (url == null)
            {
                return View("NotFound");
            }
            urlRepo.IncrementViews(url.Id);
            return Redirect(url.OriginalUrl);
        }

    }
}