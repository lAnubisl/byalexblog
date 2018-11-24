using System.Linq;
using System.Threading.Tasks;
using byalexblog.Core;
using byalexblog.DAL;
using byalexblog.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace byalexblog.Controllers
{
    public class ArticleController : Controller
    {
        private readonly IArticleDAO _dao;
        private readonly IConfigurationProvider _configurationProvider;

        public ArticleController(IArticleDAO dao, IConfigurationProvider configurationProvider)
        {
            _dao = dao;
            _configurationProvider = configurationProvider;
        }

        [HttpGet]
        public ActionResult Index()
        {
            return View("Display", _dao.Load(0, 1).FirstOrDefault());
        }

        [HttpGet]
        public ActionResult List(int page)
        {
            var articlesCount = _dao.Count();
            var articlesCountOnPage = _configurationProvider.GetArticlesOnPageCount();
            var articles = _dao.Load((page - 1) * articlesCountOnPage, articlesCountOnPage);
            return View(new ArticleListModel(new PagingModel(page, articlesCount, articlesCountOnPage), articles));
        }

        [HttpGet]
        public async Task<ContentResult> Rss()
        {
            var articles = _dao.Load(0, 10);
            var model = new RssViewModel(articles, Request);
            return new ContentResult
            {
                Content = await model.GetContent(),
                ContentType = "application/rss+xml"
            };
        }

        [HttpGet]
        public ActionResult Display(string seoUrl)
        {
            return View(_dao.Load(seoUrl));
        }

        [HttpGet, Authorize]
        public ActionResult Add()
        {
            return View("Edit", new ArticleEditModel());
        }

        [HttpPost, ValidateAntiForgeryToken, Authorize]
        public ActionResult Add(ArticleAddModel model)
        {
            return Edit(model);
        }

        [HttpGet, Authorize]
        public ActionResult Edit(string seoUrl)
        {
            return View(_dao.Load(seoUrl));
        }

        [HttpPost, ValidateAntiForgeryToken, Authorize]
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

            _dao.Save(model);
            return RedirectToAction("Display", new { seoUrl = model.URI });
        }

        [HttpPost, ValidateAntiForgeryToken, Authorize]
        public ActionResult Delete(string seoUrl)
        {
            _dao.Delete(seoUrl);
            return RedirectToAction("Index");
        }
    }
}