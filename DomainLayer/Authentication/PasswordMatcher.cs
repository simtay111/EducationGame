using System;
using System.Security.Cryptography;
using System.Text;
using DomainLayer.Entities;

namespace DomainLayer.Authentication
{
    public class PasswordMatcher : IPasswordMatcher
    {
        public bool IsMatch(string unhashedPassword, Account account)
        {
            return IsMatch(unhashedPassword, account.PasswordSalt, account.Password);
        }

        public bool IsMatch(string unhashedPassword, string salt, string password)
        {
            var passwordWithSalt = string.Concat(unhashedPassword, salt);
            var bytePassword = Encoding.Unicode.GetBytes(passwordWithSalt);
            var hasher = new SHA256Managed();

            var hashedPassword = hasher.ComputeHash(bytePassword);

            return (password == Convert.ToBase64String(hashedPassword));
        }
    }

    public interface IPasswordMatcher
    {
        bool IsMatch(string unhashedPassword, Account account);
        bool IsMatch(string unhashedPassword, string salt, string password);
    }
}