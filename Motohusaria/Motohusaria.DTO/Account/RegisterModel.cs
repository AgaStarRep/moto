using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Motohusaria.DTO
{
    public class RegisterModel
    {
        [Required(ErrorMessage = "Login jest wymagany")]
        public string Login { get; set; }

        [Required(ErrorMessage = "Hasło jest wymagane")]
        public string Password { get; set; }

    [Compare("Password", ErrorMessage = "Hasła nie pasują do siebie.")]
        public string PasswordConfirm { get; set; }
    }
}
