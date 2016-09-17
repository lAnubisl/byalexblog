using System;
using System.Collections.Generic;
using MvcApplication.DAL.Interfaces;

namespace MvcApplication.DAL
{
    internal class Article : IArticle
    {
        public string Body { get; set; }
        public string ShortBody { get; set; }
        public string Description { get; set; }
        public string Keywords { get; set; }
        public string URI { get; set; }
        public string Title { get; set; }
        public DateTime DateCreated { get; set; }
        public ICollection<string> Tags { get; set; }
        public bool IsPublished { get; set; }
    }
}
