using System;
using System.IO;
using System.Linq;
using System.Web.Hosting;
using System.Web.Mvc;

namespace MvcApplication.Controllers
{
    public class ImageModelItem
    {
        internal ImageModelItem(string path)
        {
            Path = "/" + path
                .Remove(0, HostingEnvironment.ApplicationPhysicalPath.Length)
                .Replace("\\", "/");
        }

        public string Path { get; }
    }

    public class ImageController : Controller
    {
        private static readonly string ImagePath = HostingEnvironment.ApplicationPhysicalPath + "Content";
        private static readonly string[] ImageExtensions = {".png", ".jpg", ".git"};

        private static bool IsImage(string input)
        {
            return ImageExtensions.Any(ext => input.EndsWith(ext, StringComparison.InvariantCultureIgnoreCase));
        }

        public ActionResult Index()
        {
            var model = Directory.GetFiles(ImagePath).Where(IsImage).Select(x => new ImageModelItem(x));
            return View(model);
        }
    }
}