using System;
using Domain;
using DomainLayer.Email;
using DomainLayer.Entities;
using DomainLayer.RepoInterfaces;
using DomainLayer.Stories;

namespace DomainLayer.Authentication
{
    public interface ICreateUserRequestHandler
    {
        CreateUserResponse Handle(CreateUserRequest request);
    }

    public class CreateUserRequestHandler : ICreateUserRequestHandler
    {
        private readonly EmailSender _emailSender;
        private readonly IAccountRepository _accountRepository;
        private readonly IPrizeRepository _prizeRepository;
        private readonly IPasswordHasher _passwordHasher;
        private readonly INewAccountStoryAdder _newAccountStoryAdder;

        public CreateUserRequestHandler(EmailSender emailSender, IAccountRepository accountRepository, IPrizeRepository prizeRepository, IPasswordHasher passwordHasher, INewAccountStoryAdder newAccountStoryAdder)
        {
            _emailSender = emailSender;
            _accountRepository = accountRepository;
            _prizeRepository = prizeRepository;
            _passwordHasher = passwordHasher;
            _newAccountStoryAdder = newAccountStoryAdder;
        }

        public CreateUserResponse Handle(CreateUserRequest request)
        {
            if (AllFieldsAreNotFilledOut(request))
                return new CreateUserResponse { Status = ResponseCode.RequestNotFinished };

            if (!RegexLibrary.MatchForEmail(request.UserName))
                return new CreateUserResponse { Status = ResponseCode.InvalidFormat };

            if (_accountRepository.GetByLoginEmail(request.UserName) != null)
                return new CreateUserResponse {Status = ResponseCode.EmailAlreadyInUse};

            if (PasswordAndConfirmPasswordDontMatch(request))
                return new CreateUserResponse { Status = ResponseCode.PasswordMismatch };

            var account = new Account
                              {
                                  Email = request.UserName.ToUpper(),
                                  Password = request.Password,
                                  DisplayName = request.DisplayName,
                                  PermissionLevel = request.PermissionLevel
                              };

            _passwordHasher.HashPasswordForUser(account);

            if (!string.IsNullOrEmpty(request.CreationAccount))
            {
                var accountInformation = _accountRepository.GetAccountInformation(request.CreationAccount);
                account.AccountInformation = accountInformation;
            }
            else
            {
                var accountInformation = new AccountInformation();
                accountInformation.CreationDate = DateTime.Now;
                accountInformation.DateOfPrintedDaily = DateTime.Now.AddDays(-2);
                accountInformation.CostPerQuiz = (decimal)1.0;
                accountInformation.DatePayedThrough = DateTime.Now.AddDays(-2);
                _accountRepository.SaveAccountInformation(accountInformation);
                for (int i = 1; i <= Constants.MaxPrizeCats; i++)
                {
                    var prize = new CustomPrize{AccountInformation = accountInformation, Points = 0};
                    _prizeRepository.Save(prize);
                }
                accountInformation.CostPerQuiz = (decimal)1.0;
                account.AccountInformation = accountInformation;

                _newAccountStoryAdder.AddStoriesToAccount(accountInformation);
                var body =
                    "Welcome to the PracticeOwl family and congratulations on registering with us!" +
                    "\n\nWe have big hopes for how this program will alleviate the pain of educating your patients." +
                    "\n\nPlease make certain to start using it RIGHT AWAY! It's absolutely EFFORTLESS to begin, " +
                    "just login to your dashboard and display the Daily Printout for your patients to login, that's " +
                    "all you need to do.\n\nTo making your life easier,\nYour PracticeOwl Team\n";
                _emailSender.SendEmail(account.Email, body, "Welcome to PracticeOwl");
            }

            _accountRepository.Save(account);
            return new CreateUserResponse
                       {
                           Account = account
                       };
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
    }

    public class CreateUserRequest
    {
        public string UserName { get; set; }

        public string Password { get; set; }

        public string DisplayName { get; set; }

        public string ConfirmPass { get; set; }

        public string CreationAccount { get; set; }

        public int PermissionLevel { get; set; }
    }

    public class CreateUserResponse
    {
        public ResponseCode Status { get; set; }
        public Account Account { get; set; }
    }
}