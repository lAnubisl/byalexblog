using byalexblog.DAL;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace byalexblog.Models
{
    public class SiteMapViewModel
    {
        [XmlRoot(ElementName = "url", Namespace = "http://www.sitemaps.org/schemas/sitemap/0.9")]
        public class Url
        {
            [XmlElement(ElementName = "loc", Namespace = "http://www.sitemaps.org/schemas/sitemap/0.9")]
            public string Loc { get; set; }

            [XmlElement(ElementName = "lastmod", Namespace = "http://www.sitemaps.org/schemas/sitemap/0.9")]
            public DateTime Lastmod { get; set; }

            [XmlElement(ElementName = "changefreq", Namespace = "http://www.sitemaps.org/schemas/sitemap/0.9")]
            public string Changefreq { get; set; }

            [XmlElement(ElementName = "priority", Namespace = "http://www.sitemaps.org/schemas/sitemap/0.9")]
            public decimal Priority { get; set; }
        }

        [XmlRoot(ElementName = "urlset", Namespace = "http://www.sitemaps.org/schemas/sitemap/0.9")]
        public class Urlset
        {
            [XmlElement(ElementName = "url", Namespace = "http://www.sitemaps.org/schemas/sitemap/0.9")]
            public List<Url> Url { get; set; }
        }

        private string ComposeAbsoluteUrl(string path)
        {
            return new UriBuilder()
            {
                Path = path
            }.ToString();
        }

        public string Content { get; }
        public SiteMapViewModel(IEnumerable<IArticle> articles, IUrlHelper urlHelper)
        {
            var sitemap = new Urlset();
            sitemap.Url = articles.Select(a => new Url
            {
                Loc = ComposeAbsoluteUrl(urlHelper.RouteUrl("DisplayArticle", new { seoUrl = a.URI })),
                Changefreq = "monthly",
                Lastmod = a.DateCreated,
                Priority = a.DateCreated.AddMonths(1) < DateTime.Now
                    ? new decimal(0.5)
                    : new decimal(0.8)
            }).ToList();

            sitemap.Url.Add(new Url
            {
                Loc = ComposeAbsoluteUrl(urlHelper.RouteUrl("AboutMe")),
                Changefreq = "monthly",
                Lastmod = DateTime.Now.AddMonths(-1),
                Priority = new decimal(0.9)
            });

            sitemap.Url.Add(new Url
            {
                Loc = ComposeAbsoluteUrl(urlHelper.RouteUrl("Index")),
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

            Content = sb.ToString();
        }
    }
}