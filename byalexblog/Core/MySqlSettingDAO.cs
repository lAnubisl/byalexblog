using Dapper;
using System.Linq;

namespace byalexblog.Core
{
    public class MySqlSettingDAO : BaseMySqlDAO, ISettingDAO
    {
        public MySqlSettingDAO(IConfigurationProvider configurationProvider) : base(configurationProvider)
        {
        }

        public string Get(string key)
        {
            using (var connection = NewConnection)
            {
                return connection.Query<string>("select Value from Settings where Name = @key", new { key }).First();
            }
        }
    }
}