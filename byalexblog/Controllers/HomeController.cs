using byalexblog.Core;
using byalexblog.Models;
using Microsoft.AspNetCore.Mvc;

namespace byalexblog.Controllers
{
    public class HomeController : Controller
    {
		private readonly IArticleDAO _dao;

	    public HomeController(IArticleDAO dao)
	    {
		    _dao = dao;
	    }

        [HttpGet]
	    public ViewResult AboutMe()
        {
            return View("AboutMe");
        }

        [HttpGet]
        public ContentResult SiteMap()
        {
            var articles = _dao.Load(0, int.MaxValue);
            var model = new SiteMapViewModel(articles, Url);
            return new ContentResult
            {
                Content = model.Content,
                ContentType = "application/xml"
            };
        }
    }
}