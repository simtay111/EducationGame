using System;
using System.Web.Mvc;
using DataLayer;
using DomainLayer;
using DomainLayer.Authentication;
using DomainLayer.Email;
using DomainLayer.Entities;
using EducationGame.Controllers.CustomResults;

namespace EducationGame.Controllers
{
    public class RegisterController : Controller
    {
        private readonly RegisterUserHandler _createAccountRequestHandler;
        private CreateMemberRequestHandler _createMemberRequestHandler;

        public RegisterController()
        {
            _createAccountRequestHandler =
                new CreateAccountRequestHandler(new AccountRepository(new ConnectionProvider()),
                    new PasswordHasher(), new EmailSender(new AuditLogRepository(new ConnectionProvider())));
            _createMemberRequestHandler =
                new CreateMemberRequestHandler(new MemberRepository(new ConnectionProvider()),
                    new PasswordHasher(), new EmailSender(new AuditLogRepository(new ConnectionProvider())));
        }

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public JsonResult RegisterMember(RegistrationModel model)
        {
            try
            {
                var response = _createMemberRequestHandler.Handle(new CreateUserRequest
                {
                    UserName = model.Username,
                    Password = model.Password,
                    ConfirmPass = model.ConfirmPassword,
                });
                return new JsonDotNetResult { Data = new { Successful = true } };
            }
            catch (CreateUserException exception)
            {
                return new JsonDotNetResult { Data = new { Successful = false, Reason = exception.CreateMessage } };
            }
        }

        [HttpPost]
        public JsonResult RegisterUser(RegistrationModel model)
        {
            try
            {
                var accountRepository = new AccountRepository(new ConnectionProvider());
                var response = _createAccountRequestHandler.Handle(new CreateUserRequest
                {
                    UserName = model.Username,
                    Password = model.Password,
                    ConfirmPass = model.ConfirmPassword,
                });

                var accountInformation = new AccountInformation
                {
                    CreationDate = DateTime.Now,
                    DatePayedThrough = DateTime.Now.AddDays(-2)
                };
                accountRepository.SaveAccountInformation(accountInformation);
                var account = ((Account)response.Account);
                account.PermissionLevel = RolesStatic.SuperUser;
                account.AccountInformation = accountInformation;
                Session[SessionConstants.AcctInfoId] = accountInformation.Id;
                Session[SessionConstants.AcctPermissionLevel] = RolesStatic.SuperUser;

                accountRepository.Save(response.Account);

                var loginController = new LoginRequestHandler(accountRepository,
                    new UserAuthenticator(), new PasswordMatcher());

                loginController.Handle(new LoginRequest
                {
                    UserName = model.Username,
                    Password = model.Password,
                    Session = Session
                });
                return new JsonResult
                {
                    Data =
                        new { Successful = true }
                };
            }
            catch (CreateUserException exception)
            {
                return new JsonResult { Data = new { Successful = false, Reason = exception.CreateMessage } };
            }

        }
        [HttpPost]
        public JsonResult RegisterAssistant(RegistrationModel model)
        {
            var accountRepository = new AccountRepository(new ConnectionProvider());
            var response = _createAccountRequestHandler.Handle(new CreateUserRequest
                {
                    UserName = model.Username,
                    Password = model.Password,
                    ConfirmPass = model.ConfirmPassword,
                    DisplayName = model.DisplayName,
                    PermissionLevel = RolesStatic.OfficeManager,
                    CreationAccount = User.Identity.Name
                });

            if (!string.IsNullOrEmpty(User.Identity.Name))
            {
                var accountInformation = accountRepository.GetAccountInformation(User.Identity.Name);
                ((Account)response.Account).AccountInformation = accountInformation;
            }

            return new JsonResult { Data = new { Successful = response.Status == ResponseCode.Success, Reason = response.Status.ToString() } };
        }
    }

    public class RegistrationModel
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public string DisplayName { get; set; }
    }
}
