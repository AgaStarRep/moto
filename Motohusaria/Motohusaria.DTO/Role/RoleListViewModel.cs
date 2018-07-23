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
    public class RoleListViewModel : IListViewModel
{
        public RoleListViewModel()
        {
        }
        [MaxLength(128)]
        [Required]
        public String Name { get; set; }

        public Guid? Id { get; set; }

    }
}

