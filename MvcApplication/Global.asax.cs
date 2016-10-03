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

        protected void Application_Start()
        {
            GlobalFilters.Filters.Add(new HandleErrorAttribute());
            RegisterRoutes(RouteTable.Routes);
            ControllerBuilder.Current.SetControllerFactory(new MyControllerFactory());
            RegisterComponents();
        }

        private static void RegisterComponents()
        {
            Container.Register<IConfigurationProvider, ConfigurationProvider>(Reuse.Singleton);
            Container.Register<ILoggedInUserHelper, LoggedInUserHelper>(Reuse.Singleton);
            Container.Register<IArticleDAO, MsSqlArticleDAO>(Reuse.Singleton);
            Container.Register<HomeController, HomeController>(Reuse.Transient);
            Container.Register<ArticleController, ArticleController>(Reuse.Transient);
            Container.Register<AccountController, AccountController>(Reuse.Transient);
        }

        private static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.MapRoute("Index", "", new { controller = "Article", action = "Index" });
            routes.MapRoute("SiteMap", "SiteMap", new { controller = "Home", action = "SiteMap" });
            routes.MapRoute("Rss", "Rss", new { controller = "Article", action = "Rss" });
            routes.MapRoute("AboutMe", "AboutMe", new { controller = "Home", action = "AboutMe" });
            routes.MapRoute("ListArticles", "archive/{page}", new { controller = "Article", action = "List", page = 1 }, new { page = "([\\d]+)?" });
            routes.MapRoute("login", "{action}", new { controller = "Account" }, new { action = "login|logout" });
            routes.MapRoute("AddArticle", "{action}", new { controller = "Article" }, new { action = "add|delete" });
            routes.MapRoute("DisplayArticle", "{seoUrl}", new { controller = "Article", action = "Display" });
            routes.MapRoute("EditArticle", "{seoUrl}/Edit", new { controller = "Article", action = "Edit" });
            routes.MapRoute("Default", "{controller}/{action}");
        }
    }
}