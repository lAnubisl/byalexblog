using System.ComponentModel.DataAnnotations;
using MvcApplication.Validation;

namespace MvcApplication.Models
{
    public class ArticleEditModel : ArticleAddModel
    {
        [UrlExists]
        [Required]
        [RegularExpression("^[a-z-]+$")]
        public override string URI { get; set; }
    }
}