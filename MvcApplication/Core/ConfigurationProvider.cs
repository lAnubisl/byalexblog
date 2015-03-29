using System;
using System.Configuration;
using MvcApplication.Properties;

namespace MvcApplication.Core
{
    public class ConfigurationProvider : IConfigurationProvider
    {
        public string GetConnectionString()
        {
            return ConfigurationManager.ConnectionStrings["mongodb"].ConnectionString;
        }

        public string GetAdminPassword()
        {
            return DateTime.Now.Month + ConfigurationManager.AppSettings["AdminPassword"];
        }

        public int GetArticlesOnPageCount()
        {
            return Settings.Default.ArticlesOnPage;
        }
    }
}