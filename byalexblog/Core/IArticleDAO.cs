using byalexblog.DAL;
using System.Collections.Generic;

namespace byalexblog.Core
{
    public interface IArticleDAO
    {
        int Count();

        int Count(string URI);

        IEnumerable<IArticle> Load(int skip, int take);

        IEnumerable<IArticleLink> LoadRecent(int take); 
            
        IArticle Load(string URI);

        void Save(IArticle article);

        void Delete(string URI);
    }
}