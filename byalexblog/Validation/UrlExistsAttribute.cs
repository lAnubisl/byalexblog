using System.ComponentModel.DataAnnotations;

namespace byalexblog.Validation
{
    public class UrlExistsAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            var v = value as string;
            if (string.IsNullOrWhiteSpace(v))
            {
                return false;
            }

            return true;
            //var dao = MvcApplication.Container.Resolve<IArticleDAO>();
            //return dao.Count(v) > 0;
        }
    }
}