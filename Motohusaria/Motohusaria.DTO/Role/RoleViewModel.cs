using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using Motohusaria.DomainClasses;
using System.Threading.Tasks;
namespace Motohusaria.DTO
{
    [AutoMap(typeof(Motohusaria.DomainClasses.Role))]
    public class RoleViewModel : IViewModel
{
        public RoleViewModel()
        {
        }

        [MaxLength(128, ErrorMessage = "Maksymalna długość nazwy to 128 znaków")]
        [Required(ErrorMessage = "Nazwa jest wymagana")]
        public String Name { get; set; }

        public Guid[] RoleUsersSelectedIds { get; set; }

        public Guid? Id { get; set; }


    }
}