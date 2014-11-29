using DomainLayer.Entities;

namespace DomainLayer.RepoInterfaces
{
    public interface ILogginEntityProvider : IDataAccess
    {
        IHaveAuthorizationCredentials GetByLoginEmail(string email);
    }
}