namespace MvcApplication.Core
{
    public interface IConfigurationProvider
    {
        string GetConnectionString();
        string GetAdminPassword();
        int GetArticlesOnPageCount();
    }
}
