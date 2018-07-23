using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using Motohusaria.DomainClasses;

namespace Motohusaria.Services
{
    [InjectableService(typeof(IPasswordService))]
    public class MD5PasswordService : IPasswordService
    {
        const int generatedPasswordLength = 64;
        const int generatedPasswordNonAlphanumericChars = 5;
        const int saltLength = 10;
        const int saltNonAlphanumericChars = 3;
        private readonly RNGCryptoServiceProvider cryptRNG;

        public MD5PasswordService()
        {
            cryptRNG = new RNGCryptoServiceProvider();
        }

        public string HashPassword(string password, ref string salt)
        {
            if (string.IsNullOrEmpty(salt))
            {
                salt = GetRandomText(10);
            }
            using (MD5 md5Hash = MD5.Create())
            {
                byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(password + salt));
                StringBuilder sBuilder = new StringBuilder();
                for (int i = 0; i < data.Length; i++)
                {
                    sBuilder.Append(data[i].ToString("x2"));
                }
                return sBuilder.ToString();
            }
        }

        public bool VerifyPassword(User user, string password)
        {
            return VerifyPassword(password, user.Salt, user.PasswordHash);
        }

        public bool VerifyPassword(string password, string salt, string hash)
        {
            string hashOfInput = HashPassword(password, ref salt);
            StringComparer comparer = StringComparer.OrdinalIgnoreCase;
            return 0 == comparer.Compare(hashOfInput, hash);
        }

        private string GetRandomText(int length)
        {
            byte[] rngBytes = new byte[length];
            cryptRNG.GetBytes(rngBytes);
            var text = Encoding.ASCII.GetString(rngBytes);
            return text.Substring(0, length);
        }
    }
}
