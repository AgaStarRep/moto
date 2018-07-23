using System.ComponentModel.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Text;

namespace Motohusaria.DomainClasses
{
    [Name("@@Entity@@.Name")]
    public class Role : BaseEntity
    {
        [Display(Name = "Nazwa")]
        [SearchField]
        [Required]
        [MaxLength(128)]
		[ListFilter]
        [EditLink]
        public string Name { get; set; }

        public virtual ICollection<UserRole> RoleUsers { get; set; }
    }
}