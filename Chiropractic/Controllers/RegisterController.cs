using System.Web.Mvc;
using Chiropractic;
using DataLayer;
using DomainLayer;
using DomainLayer.Authentication;
using DomainLayer.Email;
using DomainLayer.Stories;

namespace EducationGame.Controllers
{
    public class RegisterController : Controller
    {
        private readonly ICreateUserRequestHandler _createUserRequestHandler;

        public RegisterController(ICreateUserRequestHandler createUserRequestHandler)
        {
            _createUserRequestHandler = createUserRequestHandler;
        }

        public RegisterController()
        {
            _createUserRequestHandler = new CreateUserRequestHandler(new EmailSender(new AuditLogRepository(new ConnectionProvider())), new AccountRepository(new ConnectionProvider()), new PrizeRepository(new ConnectionProvider()), new PasswordHasher(), new NewAccountStoryAdder(new SlideRepository(new ConnectionProvider()), new StoryRepository(new ConnectionProvider()), new QuestionRepository(new ConnectionProvider()),new DefaultQuestionRepository(new ConnectionProvider()), new DefaultSlideRepository(new ConnectionProvider()), new DefaultStoryRepository(new ConnectionProvider())));
        }
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public JsonResult RegisterUser(RegistrationModel model)
        {
            var response = _createUserRequestHandler.Handle(new CreateUserRequest
                {
                    UserName = model.Username,
                    Password = model.Password,
                    ConfirmPass = model.ConfirmPassword,
                    PermissionLevel = RolesStatic.SuperUser
                });

            if (response.Status == ResponseCode.Success)
            {
                var loginController = new LoginRequestHandler(new AccountRepository(new ConnectionProvider()),
                                                              new UserAuthenticator(), new PasswordMatcher());

                loginController.Handle(new LoginRequest
                    {
                        UserName = model.Username,
                        Password = model.Password,
                        Session = Session
                    });
            }

            return new JsonResult {Data = new { Successful = response.Status == ResponseCode.Success, Reason = response.Status.ToString() }};
        }
        [HttpPost]
        public JsonResult RegisterAssistant(RegistrationModel model)
        {
            var response = _createUserRequestHandler.Handle(new CreateUserRequest
                {
                    UserName = model.Username,
                    Password = model.Password,
                    ConfirmPass = model.ConfirmPassword,
                    DisplayName = model.DisplayName,
                    PermissionLevel = RolesStatic.OfficeManager,
                    CreationAccount = User.Identity.Name
                });

            return new JsonResult {Data = new { Successful = response.Status == ResponseCode.Success, Reason = response.Status.ToString() }};
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
