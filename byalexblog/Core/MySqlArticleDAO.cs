using System;
using System.Collections.Generic;
using System.Linq;
using Dapper;
using byalexblog.DAL;
using MySql.Data.MySqlClient;

namespace byalexblog.Core
{
    public class MySqlArticleDAO : IArticleDAO
    {
        private readonly ILoggedInUserHelper userHelper;
        private readonly IConfigurationProvider configurationProvider;

        public MySqlArticleDAO(ILoggedInUserHelper userHelper, IConfigurationProvider configurationProvider)
        {
            this.userHelper = userHelper;
            this.configurationProvider = configurationProvider;
        }

        private MySqlConnection NewConnection => new MySqlConnection(configurationProvider.GetConnectionString());


        public int Count()
        {
            var query = "select count(*) from Articles";
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
                return connection.Query<int>("select count (1) from Articles where URI = @URI", new { URI }).First();
            }
        }

        public IEnumerable<IArticle> Load(int skip, int take)
        {
            var query = "select * from Articles";
            if (!userHelper.IsAdmin())
            {
                query = query + " where IsPublished = 1";
            }
            query = query + " order by DateCreated desc limit @skip, @take";
            using (var connection = NewConnection)
            {
                return connection.Query<Article>(query, new {skip, take}).ToList();
            }
        }

        public IEnumerable<IArticleLink> LoadRecent(int take)
        {
            using (var connection = NewConnection)
            {
                return connection.Query<Article>("select URI, Title from Articles where IsPublished = 1 order by DateCreated desc limit 0, @take", new {take}).ToList();
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
                return connection.Query<Article>("select * from Articles where URI = @URI", new { URI }).FirstOrDefault();
            }
        }

        public void Save(IArticle article)
        {
            if (article == null)
            {
                return;
            }

            var newArticle = false;
            var dbArticle = Load(article.URI);
            if (dbArticle == null)
            {
                newArticle = true;
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

        private const string InsertArticleQuery = "insert into Articles (URI, Title, Tags, Body, Keywords, Description, IsPublished, ShortBody, DateCreated) values (@URI, @Title, @Tags, @Body, @Keywords, @Description, @IsPublished, @ShortBody, @DateCreated)";
        private const string UpdateArticleQuery = "update Articles set Title = @Title, Tags = @Tags, Body = @Body, Keywords = @Keywords, Description = @Description, IsPublished = @IsPublished, ShortBody = @ShortBody, DateCreated = @DateCreated where URI = @URI";

        public void Delete(string URI)
        {
            using (var connection = NewConnection)
            {
                connection.Execute("delete from Articles where URI = @URI", new {URI});
            }
        }
    }
}