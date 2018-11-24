using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace byalexblog.DAL
{
    public interface IArticle : IArticleLink
    {
        [Required]
        string Body { get; set; }
        string Description { get; set; }
        string Keywords { get; set; }
        [Required]
        string ShortBody { get; set; }
        [Required]
        new string URI { get; set; }
        [Required]
        new string Title { get; set; }
        DateTime DateCreated { get; }
        ICollection<string> Tags { get; }
        bool IsPublished { get; set; }
    }
}