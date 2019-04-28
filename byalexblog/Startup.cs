using byalexblog.Core;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace byalexblog
{
    public class Startup
    {
        private static IConfiguration _configuration;
        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<Core.IConfigurationProvider>(new Core.ConfigurationProvider(_configuration));
            services.AddSingleton<ILoggedInUserHelper, LoggedInUserHelper>();
            services.AddTransient<IHttpContextAccessor, HttpContextAccessor>();
            services.AddSingleton<IArticleDAO, MySqlArticleDAO>();
            services.AddSingleton<ISettingDAO, MySqlSettingDAO>();
            services.AddMvc();
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie((options) => 
            {
                options.LoginPath = new PathString("/Account/Login");
                options.LogoutPath = new PathString("/Account/logout");
            });
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
            });

            app.UseStaticFiles();
            app.UseCookiePolicy();
            app.UseAuthentication();
            app.UseMvc(routes =>
            {
                routes.MapRoute("Index", "", new { controller = "Article", action = "Index" });
                routes.MapRoute("SiteMap", "SiteMap", new { controller = "Home", action = "SiteMap" });
                routes.MapRoute("Rss", "Rss", new { controller = "Article", action = "Rss" });
                routes.MapRoute("AboutMe", "AboutMe", new { controller = "Home", action = "AboutMe" });
                routes.MapRoute("ListArticles", "archive/{page}", new { controller = "Article", action = "List", page = 1 }, new { page = "([\\d]+)?" });
                routes.MapRoute("login", "{action}", new { controller = "Account" }, new { action = "login|logout" });
                routes.MapRoute("AddArticle", "{action}", new { controller = "Article" }, new { action = "add|delete" });
                routes.MapRoute("DisplayArticle", "{seoUrl}", new { controller = "Article", action = "Display" });
                routes.MapRoute("EditArticle", "{seoUrl}/Edit", new { controller = "Article", action = "Edit" });
                routes.MapRoute("Default", "{controller}/{action}");
            });
        }
    }
}