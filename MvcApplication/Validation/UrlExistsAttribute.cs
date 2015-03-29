using System.ComponentModel.DataAnnotations;
using DryIoc;
using MvcApplication.Core;

namespace MvcApplication.Validation
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

            var dao = MvcApplication.Container.Resolve<IArticleDAO>();
            return dao.Count(v) > 0;
        }
    }
}