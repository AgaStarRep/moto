using System.ComponentModel.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Text;

namespace Motohusaria.DTO
{
    public class LoginModel
    {
        [Required(ErrorMessage = "Login jest wymagany")]
        public string Login { get; set; }

        [Required(ErrorMessage = "Hasło jest wymagane")]
        public string Password { get; set; }
}
}
