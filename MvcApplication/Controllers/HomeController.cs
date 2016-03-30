using System;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Xml;
using System.Xml.Serialization;
using MvcApplication.Core;
using MvcApplication.Models.SiteMap;

namespace MvcApplication.Controllers
{
    public class HomeController : BaseController
    {
		private readonly IArticleDAO dao;

	    public HomeController(IArticleDAO dao)
	    {
		    this.dao = dao;
	    }

	    public ActionResult AboutMe()
        {
            return View("AboutMe");
        }

	    public ContentResult SiteMap()
	    {
		    var articles = dao.Load(0, int.MaxValue);
		    var sitemap = new Urlset();
		    sitemap.Url = articles.Select(a => new Url
		    {
			    Loc = ComposeAbsoluteUrl(Url.RouteUrl("DisplayArticle", new {seoUrl = a.URI})),
				Changefreq = "monthly",
				Lastmod = a.DateCreated,
				Priority = a.DateCreated.AddMonths(1) < DateTime.Now 
					? new decimal(0.5) 
					: new decimal(0.8)
			}).ToList();

			sitemap.Url.Add(new Url
			{
				Loc = ComposeAbsoluteUrl(Url.RouteUrl("AboutMe")),
				Changefreq = "monthly",
				Lastmod = DateTime.Now.AddMonths(-1),
				Priority = new decimal(0.9)
			});

			sitemap.Url.Add(new Url
			{
				Loc = ComposeAbsoluteUrl(Url.RouteUrl("Index")),
				Changefreq = "monthly",
				Lastmod = articles.Max(a => a.DateCreated),
				Priority = new decimal(0.9)
			});

			var sb = new StringBuilder();
			using (var writer = XmlWriter.Create(sb, new XmlWriterSettings { Indent = true }))
			{
				var serializer = new XmlSerializer(typeof(Urlset));
				serializer.Serialize(writer, sitemap);
				writer.Flush();
			}

			return new ContentResult
			{
				Content = sb.ToString(),
				ContentType = "application/xml",
				ContentEncoding = Encoding.UTF8
			};
	    }

	    private string ComposeAbsoluteUrl(string path)
	    {
		    return new UriBuilder(Request.Url.AbsoluteUri)
			{
				Path = path
			}.ToString();
		}
    }
}