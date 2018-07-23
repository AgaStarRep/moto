using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Motohusaria.DomainClasses
{
    [Name("@@Entity@@.HistoricDate")]
    class Calendario : BaseEntity
    {
        [SearchField]
        [MaxLength(128)]
        [Required(ErrorMessage = "Tytuł jest wymagany")]
        [ListFilter]
        [EditLink]
        public string CalendarioTitle { get; set; }

        [SearchField]
        [MaxLength(128)]
        [Required(ErrorMessage = "Data jest wymagana")]
        [ListFilter]
        [EditLink]
        public DateTime CalendarioDate { get; set; }

        [SearchField]
        [MaxLength(1000)]
        [Required(ErrorMessage = "Wpisz treść Kalendarium")]
        [ListFilter]
        [EditLink]
        public string CalendarioData { get; set; }

        public virtual ICollection<Calendario> Calendarios { get; set; }
    }
}
