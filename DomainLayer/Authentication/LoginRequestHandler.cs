using System.Web;
using DomainLayer.RepoInterfaces;

namespace DomainLayer.Authentication
{
    public class LoginRequestHandler : ILoginRequestHandler
    {
        private readonly ILogginEntityProvider _loginableEntityProvider;
        private readonly IAuthenticateUsers _authenticateUsers;
        private readonly IPasswordMatcher _passwordMatcher;

        public LoginRequestHandler(ILogginEntityProvider loginableEntityProvider, IAuthenticateUsers authenticateUsers, IPasswordMatcher passwordMatcher)
        {
            _loginableEntityProvider = loginableEntityProvider;
            _authenticateUsers = authenticateUsers;
            _passwordMatcher = passwordMatcher;
        }

        public LoginResponse Handle(LoginRequest request)
        {
            if (string.IsNullOrEmpty(request.UserName) || string.IsNullOrEmpty(request.Password))
                return new LoginResponse { Status = ResponseCode.RequestNotFinished };

            var user = _loginableEntityProvider.GetByLoginEmail(request.UserName);

            if (user == null || !_passwordMatcher.IsMatch(request.Password, user))
                return new LoginResponse { Status = ResponseCode.WrongAccountInformation };

            _authenticateUsers.AuthenticateUser(request.Session, user);
            
            return new LoginResponse
                {
                    RecordId = user.Id,
                };
        }
    }

    public interface ILoginRequestHandler
    {
        LoginResponse Handle(LoginRequest request);
    }

    public class LoginRequest
    {
        public string UserName { get; set; }

        public string Password { get; set; }

        public HttpSessionStateBase Session { get; set; }
    }

    public class LoginResponse
    {
        public ResponseCode Status { get; set; }
        public int RecordId { get; set; }
        public int AccountInfoId { get; set; }
    }
}