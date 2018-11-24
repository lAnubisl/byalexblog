using System.ComponentModel.DataAnnotations;
using byalexblog.Validation;

namespace byalexblog.Models
{
    public class ArticleEditModel : ArticleAddModel
    {
        [UrlExists]
        [Required]
        [RegularExpression("^[a-z-]+$")]
        public override string URI { get; set; }
    }
}