using System;
using System.Diagnostics;
using System.Web;
using System.Web.Security;
using Chiropractic.Filters;
using DomainLayer.Authentication;
using DomainLayer.Entities;

namespace Chiropractic
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