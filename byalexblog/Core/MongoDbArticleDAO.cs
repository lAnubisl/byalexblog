using System;
using System.Collections.Generic;
using System.Linq;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using MongoDB.Driver.Linq;
using byalexblog.DAL;
using byalexblog.DAL.Interfaces;

namespace byalexblog.Core
{
    public class MongoDbArticleDAO : IArticleDAO
    {
        private readonly MongoCollection<MongoDbArticle> articles;
        private readonly ILoggedInUserHelper userHelper;

        public MongoDbArticleDAO(ILoggedInUserHelper userHelper, IConfigurationProvider configurationProvider)
        {
            this.userHelper = userHelper;
            var url = MongoUrl.Create(configurationProvider.GetConnectionString());
            var client = new MongoClient(url);
            var server = client.GetServer();
            var database = server.GetDatabase(url.DatabaseName);
            articles = database.GetCollection<MongoDbArticle>("articles");
        }

        int IArticleDAO.Count()
        {
            var query = articles.AsQueryable();

            if (!userHelper.IsAdmin())
            {
                query = query.Where(a => a.IsPublished);
            }

            return query.Count();
        }

        int IArticleDAO.Count(string URI)
        {
            return (int)articles.Count(GetURIQuery(URI));
        }

        IEnumerable<IArticle> IArticleDAO.Load(int skip, int take)
        {
            var query = articles.AsQueryable();

            if (!userHelper.IsAdmin())
            {
                query = query.Where(a => a.IsPublished);
            }
                
            return query
                .OrderByDescending(a => a.DateCreated)
                .Skip(skip)
                .Take(take)
                .ToList();
        }

        IArticle IArticleDAO.Load(string URI)
        {
            return Load(URI);
        }

        void IArticleDAO.Save(IArticle article)
        {
            if (article == null)
            {
                return;
            }

            var dbArticle = Load(article.URI);
            if (dbArticle == null)
            {
                dbArticle = new MongoDbArticle();
                dbArticle.DateCreated = DateTime.Now;
            } 
            else if (!dbArticle.IsPublished && article.IsPublished)
            {
                dbArticle.DateCreated = DateTime.Now;
            }
            
            dbArticle.URI = article.URI;
            dbArticle.Title = article.Title;
            dbArticle.Tags = article.Tags;
            dbArticle.Body = article.Body;
            dbArticle.Keywords = article.Keywords;
            dbArticle.Description = article.Description;
            dbArticle.IsPublished = article.IsPublished;
            dbArticle.ShortBody = article.ShortBody;
            articles.Save(dbArticle);
        }

        public void Delete(string URI)
        {
            articles.Remove(GetURIQuery(URI));
        }

        private MongoDbArticle Load(string URI)
        {
            return articles.Find(GetURIQuery(URI)).FirstOrDefault();
        }

        private static IMongoQuery GetURIQuery(string URI)
        {
            return Query<MongoDbArticle>.EQ(a => a.URI, URI);
        }

        public IEnumerable<IArticleLink> LoadRecent(int take)
        {
            return articles.AsQueryable()
                .Where(a => a.IsPublished)
                .OrderByDescending(a => a.DateCreated)
                .Take(take)
                .Select(a => new MongoDbArticle { URI = a.URI, Title = a.Title})
                .ToList();
        }
    }
}