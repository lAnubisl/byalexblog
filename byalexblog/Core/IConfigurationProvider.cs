namespace byalexblog.Core
{
    public interface IConfigurationProvider
    {
        string GetConnectionString();
        string GetAdminPasswordHash();
        int GetArticlesOnPageCount();
    }
}