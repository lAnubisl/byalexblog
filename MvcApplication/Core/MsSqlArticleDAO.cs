using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using Dapper;
using MvcApplication.DAL;
using MvcApplication.DAL.Interfaces;

namespace MvcApplication.Core
{
    public class MsSqlArticleDAO : IArticleDAO
    {
        private readonly ILoggedInUserHelper userHelper;
        private readonly IConfigurationProvider configurationProvider;

        public MsSqlArticleDAO(ILoggedInUserHelper userHelper, IConfigurationProvider configurationProvider)
        {
            this.userHelper = userHelper;
            this.configurationProvider = configurationProvider;
        }

        private SqlConnection NewConnection => new SqlConnection(configurationProvider.GetConnectionString());


        public int Count()
        {
            var query = "select count (1) from article";
            if (userHelper.IsAdmin())
            {
                query = query + " where IsPublished = 1";
            }
            using (var connection = NewConnection)
            {
                return connection.Query<int>(query).First();
            }
        }

        public int Count(string URI)
        {
            using (var connection = NewConnection)
            {
                return connection.Query<int>("select count (1) from article where URI = @URI", new { URI }).First();
            }
        }

        public IEnumerable<IArticle> Load(int skip, int take)
        {
            var query = "select * from article where ";
            if (userHelper.IsAdmin())
            {
                query = query + " where IsPublished = 1";
            }
            using (var connection = NewConnection)
            {
                return connection.Query<Article>(query).ToList();
            }
        }

        public IEnumerable<IArticleLink> LoadRecent(int take)
        {
            using (var connection = NewConnection)
            {
                return connection.Query<Article>("select top @take (*) from article where IsPublished = 1 order by DateCreated desc", new {take}).ToList();
            }
        }

        IArticle IArticleDAO.Load(string URI)
        {
            return Load(URI);
        }

        private Article Load(string URI)
        {
            using (var connection = NewConnection)
            {
                return connection.Query<Article>("select * from article where URI = @URI", new { URI }).FirstOrDefault();
            }
        }

        public void Save(IArticle article)
        {
            if (article == null)
            {
                return;
            }

            var newArticle = true;
            var dbArticle = Load(article.URI);
            if (dbArticle == null)
            {
                newArticle = false;
                dbArticle = new Article();
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
            using (var connection = NewConnection)
            {
                connection.Execute(newArticle ? InsertArticleQuery : UpdateArticleQuery, dbArticle);
            }
        }

        private const string InsertArticleQuery = "insert into article (URI, Title, Tags, Body, Keywords, Description, IsPublished, ShortBody) values (@URI, @Title, @Tags, @Body, @Keywords, @Description, @IsPublished, @ShortBody)";
        private const string UpdateArticleQuery = "update article set Title = @Title, Tags = @Tags, Body = @Body, Keywords = ";

        public void Delete(string URI)
        {
            using (var connection = NewConnection)
            {
                connection.Execute("delete from article where URI = @URI", new {URI});
            }
        }
    }
}