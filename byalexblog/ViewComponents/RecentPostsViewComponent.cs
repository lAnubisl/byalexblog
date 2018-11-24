using byalexblog.Core;
using Microsoft.AspNetCore.Mvc;

namespace byalexblog.ViewComponents
{
    public class RecentPostsViewComponent : ViewComponent
    {
        private readonly IArticleDAO _dao;

        public RecentPostsViewComponent(IArticleDAO dao)
        {
            _dao = dao;
        }

        public IViewComponentResult Invoke()
        {
            return View(_dao.LoadRecent(5));
        }
    }
}