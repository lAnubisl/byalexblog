namespace byalexblog.Core
{
    public interface IConfigurationProvider
    {
        string GetConnectionString();
        int GetArticlesOnPageCount();
    }
}