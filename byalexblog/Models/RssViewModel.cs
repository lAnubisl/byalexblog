using byalexblog.DAL;
using Microsoft.AspNetCore.Http;
using Microsoft.SyndicationFeed;
using Microsoft.SyndicationFeed.Rss;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace byalexblog.Models
{
    public class RssViewModel
    {
        private class StringWriterWithEncoding : StringWriter
        {
            private readonly Encoding _encoding;

            public StringWriterWithEncoding(Encoding encoding)
            {
                _encoding = encoding;
            }

            public override Encoding Encoding
            {
                get { return _encoding; }
            }
        }

        private readonly IEnumerable<IArticle> _articles;
        private readonly HttpRequest _request;

        public RssViewModel(IEnumerable<IArticle> articles, HttpRequest request)
        {
            _articles = articles;
            _request = request;
        }

        public async Task<string> GetContent()
        {
            var sw = new StringWriterWithEncoding(Encoding.UTF8);
            using (XmlWriter xmlWriter = XmlWriter.Create(sw))
            {
                var writer = new RssFeedWriter(xmlWriter);

                foreach (var article in _articles)
                {
                    var item = new SyndicationItem
                    {
                        Title = article.Title,
                        Id = article.URI,
                        Published = article.DateCreated,
                        LastUpdated = article.DateCreated
                    };

                    item.AddLink(new SyndicationLink(new Uri(_request.Scheme + "://" + _request.Host + article.URI)));
                    await writer.Write(item);
                }

                xmlWriter.Flush();
            }

            return sw.ToString();
        }
    }
}