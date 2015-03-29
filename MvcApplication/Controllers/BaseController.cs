using System.Web;
using System.Web.Mvc;

namespace MvcApplication.Controllers
{
    public abstract class BaseController : Controller
    {
        protected new ViewResult View(string viewName, object model)
        {
            Throw404IfNull(model);
            return base.View(viewName, model);
        }

        protected new ViewResult View(object model)
        {
            Throw404IfNull(model);
            return base.View(model);
        }

        protected override PartialViewResult PartialView(string viewName, object model)
        {
            Throw404IfNull(model);
            return base.PartialView(viewName, model);
        }

        protected static void Throw404IfNull(object model)
        {
            if (model == null)
            {
                throw new HttpException(404, "Object not found");
            }
        }
    }
}