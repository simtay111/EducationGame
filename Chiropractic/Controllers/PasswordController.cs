using System;
using System.Web.Mvc;
using DataLayer;
using DomainLayer.Authentication;
using DomainLayer.Email;
using DomainLayer.Entities;
using EducationGame.Controllers.CustomResults;

namespace EducationGame.Controllers
{
    public class PasswordController : Controller
    {
        [HttpPost]
        public JsonDotNetResult ForgotPassword(ForgotPasswordModel model)
        {
            var username = model.Username;
            if (string.IsNullOrEmpty(username))
                return new JsonDotNetResult { Data = new { successful = false } };

            var userRepository = new AccountRepository(new ConnectionProvider());
            var user = userRepository.GetByLoginEmail(username);
            if (user != null)
                CreateTokenForUser(user.Email);

            return new JsonDotNetResult { Data = new { successful = true } };
        }

        private static void CreateTokenForUser(string username)
        {
            var emailSender = new EmailSender(new AuditLogRepository(new ConnectionProvider()));

            var forgotPasswordToken = new ForgotPasswordToken
                {
                    Email = username.ToUpper(),
                    UniqueToken = new Random().Next(1000, 10000).ToString()
                };

            emailSender.SendForgotPasswordToken(username, forgotPasswordToken);

            var repo = new ForgotPasswordTokenRepository(new ConnectionProvider());

            var existingToken = repo.GetForUser(username);
            if (existingToken != null)
                repo.Delete(existingToken.Id);

            repo.Save(forgotPasswordToken);
        }

        [HttpPost]
        public JsonDotNetResult ChangePassword(ChangePasswordModel model)
        {
            var forgotPasswordTokenRepository = new ForgotPasswordTokenRepository(new ConnectionProvider());
            var token = forgotPasswordTokenRepository.GetForUser(model.Email);

            if (token == null)
                return new JsonDotNetResult { Data = new { successful = false, reason = "NoTokenForUser" } };

            if (token.UniqueToken != model.UniqueToken)
                return new JsonDotNetResult { Data = new { successful = false, reason = "badToken" } };

            if (model.Password != model.ConfirmPassword)
                return new JsonDotNetResult { Data = new { successful = false, reason = "password" } };

            if (model.Password.Length < 6)
                return new JsonDotNetResult { Data = new { successful = false, reason = "invalidPassword" } };

            var hasher = new PasswordHasher();
            var userRepository = new AccountRepository(new ConnectionProvider());
            var user = userRepository.GetByLoginEmail(token.Email);
            user.Password = model.Password;
            hasher.HashPasswordForUser(user);
            userRepository.Save(user);

            forgotPasswordTokenRepository.Delete(token.Id);

            return new JsonDotNetResult { Data = new { successful = true } };
        }
    }

    public class ForgotPasswordModel
    {
        public string Username { get; set; }
    }

    public class ChangePasswordModel
    {
        public string Email { get; set; }
        public string UniqueToken { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
    }

    public class ChangePasswordResult
    {

    }
}