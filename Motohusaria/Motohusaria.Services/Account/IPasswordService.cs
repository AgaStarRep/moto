using System;
using System.Collections.Generic;
using System.Text;
using Motohusaria.DomainClasses;

namespace Motohusaria.Services
{
    public interface IPasswordService
    {
        /// <summary>
        /// Metoda hashująca hasło, jeżeli nie została podana sól to ją generuje
        /// </summary>
        /// <param name="password">Hasło, tekst do hashowania</param>
        /// <param name="salt">Sól dodawana hasła, jeżeli jest pusta do zostaje wygenerowana</param>
        /// <returns>Hash hasła</returns>
        string HashPassword(string password, ref string salt);

        /// <summary>
        /// Sprawdza czy hasło i sól jest równa hashowi
        /// </summary>
        /// <param name="password"></param>
        /// <param name="salt"></param>
        /// <param name="hash"></param>
        /// <returns></returns>
        bool VerifyPassword(string password, string salt, string hash);

        /// <summary>
        /// Sprawdza zhashowane hasło z solą jest takie same jak podane hasło
        /// </summary>
        /// <param name="password"></param>
        /// <param name="salt"></param>
        /// <param name="hash"></param>
        /// <returns></returns>
        bool VerifyPassword(User user, string password);
    }
}
