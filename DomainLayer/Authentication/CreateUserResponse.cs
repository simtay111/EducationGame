using DomainLayer.Entities;

namespace DomainLayer.Authentication
{
    public class CreateUserResponse
    {
        public ResponseCode Status { get; set; }
        public IHaveAuthorizationCredentials Account { get; set; }
    }
}