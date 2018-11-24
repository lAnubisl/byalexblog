﻿using Microsoft.Extensions.Configuration;

namespace byalexblog.Core
{
    public class ConfigurationProvider : IConfigurationProvider
    {
        private IConfiguration _configuration;

        public ConfigurationProvider(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string GetConnectionString()
        {
            return _configuration.GetConnectionString("default");
        }

        public string GetAdminPasswordHash()
        {
            return _configuration.GetSection("AppSettings")["PasswordHash"];
        }

        public int GetArticlesOnPageCount()
        {
            return 5;
        }
    }
}