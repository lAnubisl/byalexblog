using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using DryIoc;
using MvcApplication.Controllers;
using MvcApplication.Core;

namespace MvcApplication
{
    public class MvcApplication : HttpApplication
    {
        public static readonly Container Container = new Container();

        private static void RegisterComponents()
        {
            Container.Register<IConfigurationProvider, ConfigurationProvider>(Reuse.Singleton);
            Container.Register<ILoggedInUserHelper, LoggedInUserHelper>(Reuse.Singleton);
            Container.Register<IArticleDAO, ArticleDAO>(Reuse.Singleton);
            Container.Register<ArticleController, ArticleController>(Reuse.Transient);
            Container.Register<AccountController, AccountController>(Reuse.Transient);
        }

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            ControllerBuilder.Current.SetControllerFactory(new MyControllerFactory());
            RegisterComponents();
        }
    }
}