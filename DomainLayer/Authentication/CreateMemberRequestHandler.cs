using System;
using Domain;
using DomainLayer.Email;
using DomainLayer.Entities;
using DomainLayer.RepoInterfaces;

namespace DomainLayer.Authentication
{
    public abstract class RegisterUserHandler
    {
        protected readonly ILogginEntityProvider LogginEntityProvider;
        protected readonly IPasswordHasher PasswordHasher;
        protected readonly IEmailSender EmailSender;

        protected RegisterUserHandler(ILogginEntityProvider logginEntityProvider, IPasswordHasher passwordHasher, IEmailSender emailSender)
        {
            LogginEntityProvider = logginEntityProvider;
            PasswordHasher = passwordHasher;
            EmailSender = emailSender;
        }

        public CreateUserResponse Handle(CreateUserRequest request)
        {
            ValidateRequest(request);

            var record = CreateEntityFromRequest(request);

            HashPasswordAndSave(record);
           
            SendEmail(record);

            return new CreateUserResponse{Account = record};
        }

        protected abstract void SendEmail(IHaveAuthorizationCredentials record);

        private void ValidateRequest(CreateUserRequest request)
        {
            if (AllFieldsAreNotFilledOut(request))
                throw new CreateUserException("All the fields must be filled out");

            if (!RegexLibrary.MatchForEmail(request.UserName))
                throw new CreateUserException("Please enter a valid email address");

            if (GetUserByEmail(request.UserName) != null)
                throw new CreateUserException("That email appears to already be in use.");

            if (PasswordAndConfirmPasswordDontMatch(request))
                throw new CreateUserException("Your password and confirm password do not match");
        }

        private IHaveAuthorizationCredentials GetUserByEmail(string userName)
        {
            return LogginEntityProvider.GetByLoginEmail(userName);
        }

        private static bool PasswordAndConfirmPasswordDontMatch(CreateUserRequest request)
        {
            return request.Password != request.ConfirmPass;
        }

        private static bool AllFieldsAreNotFilledOut(CreateUserRequest request)
        {
            return string.IsNullOrEmpty(request.UserName) ||
                   string.IsNullOrEmpty(request.Password) ||
                   string.IsNullOrEmpty(request.ConfirmPass);
        }


        private void HashPasswordAndSave(IHaveAuthorizationCredentials record)
        {
            PasswordHasher.HashPasswordForUser(record);
            LogginEntityProvider.Save(record);
        }
        protected abstract IHaveAuthorizationCredentials CreateEntityFromRequest(CreateUserRequest request);
    }

    public class CreateUserException : Exception
    {
        public string CreateMessage { get; set; }
        public CreateUserException(string message)
        {
            CreateMessage = message;
        }  
    }

    public class CreateMemberRequestHandler : RegisterUserHandler
    {
        public CreateMemberRequestHandler(ILogginEntityProvider memberRepository, IPasswordHasher passwordHasher, IEmailSender emailSender) : base(memberRepository, passwordHasher, emailSender)
        {
        }

        protected override void SendEmail(IHaveAuthorizationCredentials record)
        {
        }

        protected override IHaveAuthorizationCredentials CreateEntityFromRequest(CreateUserRequest request)
        {
            return new Member
                              {
                                  Email = request.UserName.ToUpper(),
                                  Password = request.Password,
                              };
        }
    }

    public class CreateAccountRequestHandler : RegisterUserHandler
    {
        public CreateAccountRequestHandler(ILogginEntityProvider logginEntityProvider, IPasswordHasher passwordHasher, IEmailSender emailSender) : base(logginEntityProvider, passwordHasher, emailSender)
        {
        }

        protected override void SendEmail(IHaveAuthorizationCredentials record)
        {
            var body =
                "Welcome to the PracticeOwl family and congratulations on registering with us!" +
                "\n\nWe have big hopes for how this program will alleviate the pain of educating your patients." +
                "\n\nPlease make certain to start using it RIGHT AWAY! It's absolutely EFFORTLESS to begin, " +
                "just login to your dashboard and display the Daily Printout for your patients to login, that's " +
                "all you need to do.\n\nTo making your life easier,\nYour PracticeOwl Team\n";
            EmailSender.SendEmail(record.Email, body, "Welcome to PracticeOwl");
        }

        protected override IHaveAuthorizationCredentials CreateEntityFromRequest(CreateUserRequest request)
        {
            return new Account
            {
                Password = request.Password,
                Email = request.UserName.ToUpper(),
                DisplayName = request.DisplayName,
                PermissionLevel = request.PermissionLevel
            };
        }
    }
}