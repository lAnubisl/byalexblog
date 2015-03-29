using System;
using System.Web.Mvc;
using DryIoc;

namespace MvcApplication.Core
{
    public class MyControllerFactory : DefaultControllerFactory
    {
        protected override IController GetControllerInstance(System.Web.Routing.RequestContext requestContext, Type controllerType)
        {
            if (controllerType != null && MvcApplication.Container.IsRegistered(controllerType))
            {
                return (IController)MvcApplication.Container.Resolve(controllerType);
            }

            return base.GetControllerInstance(requestContext, controllerType);
        }
    }
}