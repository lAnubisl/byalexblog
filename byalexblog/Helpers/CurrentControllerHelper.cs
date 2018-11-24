using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System;

namespace byalexblog.Helpers
{
    public static class CurrentControllerHelper
    {
        public static bool IsControllerAndAction(this IHtmlHelper helper, string actionName, string controllerName)
        {
            return string.Equals(helper.GetDescriptor().ControllerName, controllerName, StringComparison.OrdinalIgnoreCase) &&
                   string.Equals(helper.GetDescriptor().ActionName, actionName, StringComparison.OrdinalIgnoreCase);
        }

        private static ControllerActionDescriptor GetDescriptor(this IHtmlHelper helper)
        {
            return (ControllerActionDescriptor)((HtmlHelper)helper).ViewContext.ActionDescriptor;
        }
    }
}