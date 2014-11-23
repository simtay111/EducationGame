using System.Web;
using System.Web.Security;
using DomainLayer.Authentication;
using DomainLayer.Entities;
using EducationGame.Filters;

namespace EducationGame
{
    public class UserAuthenticator : IAuthenticateUsers
    {
        public void AuthenticateUser(HttpSessionStateBase session, Account account)
        {
            FormsAuthentication.SetAuthCookie(account.Email, true);
            var setter = new SessionDataSetter();
            setter.SetOnSession(session, account);
        }
    }
}