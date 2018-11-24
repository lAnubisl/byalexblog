using byalexblog.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace byalexblog.Controllers
{
    public class ImageController : Controller
    {
        private readonly IHostingEnvironment _hostingEnvironment;
        private static readonly string[] ImageExtensions = { ".png", ".jpg", ".git" };

        private static bool IsImage(string input)
        {
            return ImageExtensions.Any(ext => input.EndsWith(ext, StringComparison.InvariantCultureIgnoreCase));
        }

        public ImageController(IHostingEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
        }

        private IEnumerable<ImageModelItem> Items =>
            Directory.GetFiles(Path.Combine(_hostingEnvironment.WebRootPath, "Content"))
            .Where(IsImage)
            .Select(x => new ImageModelItem(x.Remove(0, _hostingEnvironment.WebRootPath.Length)));

        public ActionResult Index()
        {
            return View(Items);
        }
    }
}