using System.Web.Mvc;

namespace MvcApplication.Helpers
{
    public static class CurrentControllerHelper
    {
        public static bool IsControllerAndAction(this HtmlHelper helper, string actionName, string controllerName)
        {
            return helper.GetValue("controller") == controllerName &&
                   helper.GetValue("action") == actionName;
        }

        private static string GetValue(this HtmlHelper helper, string val)
        {
            return helper.ViewContext.Controller.ValueProvider.GetValue(val).RawValue.ToString();
        }
    }
}