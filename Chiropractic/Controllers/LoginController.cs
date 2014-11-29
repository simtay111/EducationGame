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
        private readonly ILoginRequestHandler _loginRequestHandler;

        public LoginController(ILoginRequestHandler loginRequestHandler)
        {
            _loginRequestHandler = loginRequestHandler;
        }

        public LoginController()
        {
            _loginRequestHandler = new LoginRequestHandler(new AccountRepository(new ConnectionProvider()), new UserAuthenticator(),
                                                       new PasswordMatcher());
        }

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        //[AllowCrossSiteJson]
        public JsonResult Login(LoginModel loginModel)
        {
            var response = _loginRequestHandler.Handle(new LoginRequest {Password = loginModel.Password, UserName = loginModel.Username, Session = Session});

            var wasSuccessful = (response.Status != ResponseCode.Success) ? false : true;
             if (wasSuccessful)
             {
                 Session.Add(SessionConstants.AccountId, response.RecordId);
                 Session.Add(SessionConstants.AcctInfoId, response.AccountInfoId);
             }

            return new JsonResult { Data = new {successfulLogin = wasSuccessful, reason = response.Status.ToString() }, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        //[HttpPost]
        public void Logout()
        {
            FormsAuthentication.SignOut();
        }

        [HttpPost]
        public JsonDotNetResult CheckAuthorization()
        {
            return new JsonDotNetResult {Data = new {isAuthorized = HttpContext.User.Identity.IsAuthenticated}};
        }
    }

    public class LoginModel
    {
        public string Username { get; set; }
        public string Password { get; set; }

        public bool SuccessfulLogin { get; set; }
    }
}