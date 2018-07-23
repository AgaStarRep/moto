using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Motohusaria.DomainClasses
{
    [Name("@@Entity@@.Post")]
    class Article : BaseEntity
    {
        [SearchField]
        [MaxLength(128)]
        [Required(ErrorMessage = "Tytuł jest wymagany")]
        [ListFilter]
        [EditLink]
        public string ArticleTitle { get; set; }

        [SearchField]
        [MaxLength(1000)]
        [Required(ErrorMessage = "Wpisz treść artykułu")]
        [ListFilter]
        [EditLink]
        public string ArticleData { get; set; }

        public virtual ICollection<Article> Articles { get; set; }
    }
}
