using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using Motohusaria.DomainClasses;
using System.Threading.Tasks;
namespace Motohusaria.DTO
{
    [AutoMap(typeof(Motohusaria.DomainClasses.User))]
    public class UserViewModel : IViewModel
{
    public UserViewModel()
    {
    }

    [MaxLength(128, ErrorMessage = "Maksymalna długość loginu to 128 znaków")]
    [Required(ErrorMessage = "Login jest wymagany")]
    public String Login { get; set; }

    [Required(ErrorMessage = "Hasło jest wymagane")]
    public string Password { get; set; }

    [Required(ErrorMessage = "Potwierdzenie hasła jest wymagane")]
    [Compare(nameof(Password), ErrorMessage = "Hasła są różne")]
    public string PasswordConfirm { get; set; }

    public Guid[] UserRolesSelectedIds { get; set; }

    public Guid? Id { get; set; }


}
}