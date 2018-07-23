using System.ComponentModel.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Text;

namespace Motohusaria.DomainClasses
{
    [Name("@@Entity@@.Login")]
    public class User : BaseEntity
    {
        [SearchField]
        [MaxLength(128)]
        [Required(ErrorMessage = "Login jest wymagany")]
		[ListFilter]
        [EditLink]
        public string Login { get; set; }

        [Required]
        [MaxLength(32)]
        [FormIgnore]
        [ListIgnore]
        public string PasswordHash { get; set; }

        [Required]
        [FormIgnore]
        [ListIgnore]
        [MaxLength(10)]
    public string Salt { get; set; }

        public virtual ICollection<UserRole> UserRoles { get; set; }
    }
}
