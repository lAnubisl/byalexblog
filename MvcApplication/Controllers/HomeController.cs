using System.Web.Mvc;

namespace MvcApplication.Controllers
{
    public class HomeController : BaseController
    {
        public ActionResult AboutMe()
        {
            return View("AboutMe");
        }
    }
}