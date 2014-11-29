using System;
using System.Web.Mvc;
using DataLayer;
using DomainLayer;
using DomainLayer.Authentication;
using DomainLayer.Email;
using DomainLayer.Entities;

namespace EducationGame.Controllers
{
    public class RegisterController : Controller
    {
        private readonly RegisterUserHandler _createUserRequestHandler;

        public RegisterController(RegisterUserHandler createUserRequestHandler)
        {
            _createUserRequestHandler = createUserRequestHandler;
        }

        public RegisterController()
        {
            _createUserRequestHandler =
                new CreateAccountRequestHandler(new AccountRepository(new ConnectionProvider()), 
                    new PasswordHasher(), new EmailSender(new AuditLogRepository(new ConnectionProvider())));
        }
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public JsonResult RegisterUser(RegistrationModel model)
        {
            try
            {
                var accountRepository = new AccountRepository(new ConnectionProvider());
                var response = _createUserRequestHandler.Handle(new CreateUserRequest
                {
                    UserName = model.Username,
                    Password = model.Password,
                    ConfirmPass = model.ConfirmPassword,
                    PermissionLevel = RolesStatic.SuperUser
                });


                var accountInformation = new AccountInformation();
                accountInformation.CreationDate = DateTime.Now;
                accountInformation.DatePayedThrough = DateTime.Now.AddDays(-2);
                accountRepository.SaveAccountInformation(accountInformation);
                for (int i = 1; i <= Constants.MaxPrizeCats; i++)
                {
                    var prize = new CustomPrize { AccountInformation = accountInformation, Points = 0 };
                    new PrizeRepository(new ConnectionProvider()).Save(prize);
                }
                ((Account)response.Account).AccountInformation = accountInformation;

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
            var response = _createUserRequestHandler.Handle(new CreateUserRequest
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
