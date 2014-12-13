using System.Web.Mvc;
using System.Web.Security;
using DataLayer;
using DomainLayer;
using DomainLayer.Authentication;
using EducationGame.Controllers.CustomResults;

namespace EducationGame.Controllers
{
    public class LoginController : Controller
    {
        private readonly ILoginRequestHandler _loginAccountRequestHandler;
        private LoginRequestHandler _loginMemberRequestHandler;

        public LoginController(ILoginRequestHandler loginAccountRequestHandler)
        {
            _loginAccountRequestHandler = loginAccountRequestHandler;
        }

        public LoginController()
        {
            _loginAccountRequestHandler = new LoginRequestHandler(new AccountRepository(new ConnectionProvider()), new UserAuthenticator(),
                                                       new PasswordMatcher());
            _loginMemberRequestHandler = new LoginRequestHandler(new MemberRepository(new ConnectionProvider()), new UserAuthenticator(),
                                                       new PasswordMatcher());
        }

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public JsonResult LoginAccount(LoginModel loginModel)
        {
            LoginResponse response = null;
            if (loginModel.IsAccount)
                response = _loginAccountRequestHandler.Handle(new LoginRequest { Password = loginModel.Password, UserName = loginModel.Username, Session = Session });
            else
            {
                response = _loginMemberRequestHandler.Handle(new LoginRequest { Password = loginModel.Password, UserName = loginModel.Username, Session = Session });
            }

            var wasSuccessful = (response.Status == ResponseCode.Success);
            if (wasSuccessful)
            {
                Session.Add(SessionConstants.AccountId, response.RecordId);
                Session.Add(SessionConstants.IsAccount, loginModel.IsAccount);
            }

            return new JsonResult { Data = new { successfulLogin = wasSuccessful, reason = response.Status.ToString() }, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        public void Logout()
        {
            FormsAuthentication.SignOut();
        }

        [HttpPost]
        public JsonDotNetResult CheckAuthorization()
        {
            return new JsonDotNetResult { Data = new { isAuthorized = HttpContext.User.Identity.IsAuthenticated } };
        }
    }

    public class LoginModel
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public bool IsAccount { get; set; }
        public bool SuccessfulLogin { get; set; }
    }
}