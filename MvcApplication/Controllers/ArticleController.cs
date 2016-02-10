using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.ServiceModel.Syndication;
using System.Text;
using System.Web.Mvc;
using System.Xml;
using MvcApplication.Core;
using MvcApplication.DAL.Interfaces;
using MvcApplication.Models;

namespace MvcApplication.Controllers
{
    public class ArticleController : BaseController
    {
        private readonly IArticleDAO dao;
        private readonly IConfigurationProvider configurationProvider;

        public ArticleController(IArticleDAO dao, IConfigurationProvider configurationProvider)
        {
            this.dao = dao;
            this.configurationProvider = configurationProvider;
        }

        [HttpGet]
        public ActionResult Index()
        {
            var articles = dao.Load(0, 1);
            Throw404IfNull(articles);
            return View("Display", articles.FirstOrDefault());
        }

        [HttpGet]
        public ActionResult List(int page)
        {
            var articlesCount = dao.Count();
            var articlesCountOnPage = configurationProvider.GetArticlesOnPageCount();
            var articles = dao.Load((page - 1) * articlesCountOnPage, articlesCountOnPage);
            Throw404IfNull(articles);
            return View(new ArticleListModel(new PagingModel(page, articlesCount), articles));
        }

	    public ContentResult Rss()
	    {
		    var feed = new SyndicationFeed
		    {
			    Title = SyndicationContent.CreatePlaintextContent("ByAlex Code Blog"),
			    Description = SyndicationContent.CreatePlaintextContent("Alexander Panfilenok personal code blog"),
				Copyright = SyndicationContent.CreatePlaintextContent("Copyright Alexander Panfilenok"),
				Language = "en-us",
				Categories = { new SyndicationCategory(".NET"), new SyndicationCategory("Development") },
				Links = { SyndicationLink.CreateSelfLink(new Uri(Request.Url.AbsoluteUri)) },
				Items = dao.Load(0, 10)
					.Select(a => new SyndicationItem
					{
						Title = SyndicationContent.CreatePlaintextContent(a.Title),
						Id = a.URI,
						PublishDate = a.DateCreated,
						LastUpdatedTime = a.DateCreated,
						Links = { SyndicationLink.CreateAlternateLink(new Uri(new Uri(Request.Url.AbsoluteUri), a.URI)) },
						Content = SyndicationContent.CreateHtmlContent(a.Body),
						Summary = SyndicationContent.CreateHtmlContent(a.ShortBody)
					}
						)
					.ToList()
			};
		    
			var rssFormatter = new Rss20FeedFormatter(feed, false);
			var sb = new StringBuilder();
			using (var writer = XmlWriter.Create(sb, new XmlWriterSettings { Indent = true }))
			{
				rssFormatter.WriteTo(writer);
				writer.Flush();
			}

			return new ContentResult
			{
				Content = sb.ToString(),
				ContentType = "application/rss+xml"
			};
	    }

        [HttpGet]
        public ActionResult Display(string seoUrl)
        {
            return View(dao.Load(seoUrl));
        }

        [HttpGet]
        [Authorize]
        public ActionResult Add()
        {
            return View("Edit", new ArticleEditModel());
        }

        [HttpPost]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Add(ArticleAddModel model)
        {
            return Edit(model);
        }

        [HttpGet]
        [Authorize]
        public ActionResult Edit(string seoUrl)
        {
            return View("Edit", dao.Load(seoUrl));
        }

        [HttpPost]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Edit(ArticleEditModel model)
        {
            return Edit(model as IArticle);
        }

        [NonAction]
        private ActionResult Edit(IArticle model)
        {
            if (!ModelState.IsValid)
            {
                return View("Edit", model);
            }

            dao.Save(model);
            return RedirectToAction("Display", new { seoUrl = model.URI });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Delete(string seoUrl)
        {
            dao.Delete(seoUrl);
            return RedirectToAction("Index");
        }

        [ChildActionOnly]
        public ActionResult RecentPosts()
        {
            return PartialView("RecentPosts", dao.LoadRecent(5));
        }
    }
}