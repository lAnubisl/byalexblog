using System.Web.Mvc;
using System.Web.Routing;

namespace MvcApplication
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.MapRoute("Index", "", new { controller = "Article", action = "Index" });
	        routes.MapRoute("Rss", "Rss", new {controller = "Article", action = "Rss"});
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