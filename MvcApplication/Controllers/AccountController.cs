using System.Web.Mvc;
using System.Web.Security;
using MvcApplication.Core;

namespace MvcApplication.Controllers
{
    public class AccountController : Controller
    {
        private readonly IConfigurationProvider configurationProvider;

        public AccountController(IConfigurationProvider configurationProvider)
        {
            this.configurationProvider = configurationProvider;
        }

        [HttpGet]
        public ViewResult Login()
        {
            return View("Login");
        }

        [HttpGet]
        public RedirectToRouteResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Article");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(string key)
        {
            if (key != configurationProvider.GetAdminPassword())
            {
                ModelState.AddModelError("key", "Key is incorrect");
                return View("Login");
            }

            FormsAuthentication.SetAuthCookie("BlogAdmin", true);
            return Redirect(FormsAuthentication.GetRedirectUrl("BlogAdmin", true));
        }
    }
}