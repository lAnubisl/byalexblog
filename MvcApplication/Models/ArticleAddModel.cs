using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using MvcApplication.DAL.Interfaces;

namespace MvcApplication.Models
{
    public class ArticleAddModel : IArticle
    {
        public string Body { get; set; }
        [Required]
        public string ShortBody { get; set; }
        [Required]
        [RegularExpression("^[a-z0-9-]+$")]
        public virtual string URI { get; set; }
        [Required]
        public string Title { get; set; }
        public string Description { get; set; }
        public string Keywords { get; set; }
        public DateTime DateCreated { get { return DateTime.Now; } }
        public ICollection<string> Tags { get; set; }
        public bool IsPublished { get; set; }
    }
}