using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using Motohusaria.DTO;
using Motohusaria.DomainClasses;
using System.Threading.Tasks;
namespace Motohusaria.DTO
{
    [AutoMap(typeof(Motohusaria.DomainClasses.User))]
    public class UserListViewModel : IListViewModel
{
    public UserListViewModel()
    {
    }
    [MaxLength(128)]
    [Required]
    public String Login { get; set; }

    public Guid? Id { get; set; }


}
}