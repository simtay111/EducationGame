using System.Web;
using DomainLayer.Entities;

namespace DomainLayer.Authentication
{
    public interface IAuthenticateUsers
    {
        void AuthenticateUser(HttpSessionStateBase session, Account account);
    }
}