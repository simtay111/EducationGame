
using System.Collections.Generic;

namespace DomainLayer.Entities
{
    public class Member : IHaveAuthorizationCredentials
    {
        public virtual int Id { get; set; }
        public virtual string FirstName { get; set; }
        public virtual string LastName { get; set; }
        public virtual bool Inactive { get; set; }
        public string Password { get; set; }
        public string PasswordSalt { get; set; }
        public string Email { get; set; }
    }
}
