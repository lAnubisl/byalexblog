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

        public string GetAdminPasswordHash()
        {
            return ConfigurationManager.AppSettings["AdminPassword"];
        }

        public int GetArticlesOnPageCount()
        {
            return Settings.Default.ArticlesOnPage;
        }
    }
}