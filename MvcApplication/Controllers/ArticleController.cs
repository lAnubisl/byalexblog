using System.Linq;
using System.Web.Mvc;
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