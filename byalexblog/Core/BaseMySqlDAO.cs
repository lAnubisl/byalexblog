using MySql.Data.MySqlClient;
using System.Data;

namespace byalexblog.Core
{
    public class BaseMySqlDAO
    {
        private readonly IConfigurationProvider configurationProvider;

        public BaseMySqlDAO(IConfigurationProvider configurationProvider)
        {
            this.configurationProvider = configurationProvider;
        }

        protected IDbConnection NewConnection => new MySqlConnection(configurationProvider.GetConnectionString());
    }
}