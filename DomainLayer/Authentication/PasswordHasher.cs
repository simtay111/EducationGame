using System;
using System.Security.Cryptography;
using System.Text;
using DomainLayer.Entities;

namespace DomainLayer.Authentication
{
    public class PasswordHasher : IPasswordHasher
    {
        public void HashPasswordForUser(IHaveAuthorizationCredentials user)
        {
            var saltBytes = GetSaltBytes();

            user.PasswordSalt = Convert.ToBase64String(saltBytes);
            var passwordWithSalt = string.Concat(user.Password, user.PasswordSalt);
            var bytePassword = Encoding.Unicode.GetBytes(passwordWithSalt);
            var hasher = new SHA256Managed();

            var hashedPassword = hasher.ComputeHash(bytePassword);

            user.Password = Convert.ToBase64String(hashedPassword);
        }

        private static byte[] GetSaltBytes()
        {
            var saltProvider = new RNGCryptoServiceProvider();

            var saltBytes = new byte[8];
            saltProvider.GetBytes(saltBytes);
            return saltBytes;
        }
    }

    public interface IPasswordHasher
    {
        void HashPasswordForUser(IHaveAuthorizationCredentials user);
    }
}