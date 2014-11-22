using Newtonsoft.Json;

namespace DomainLayer.Entities
{
    public class Account : IHaveAuthorizationCredentials
    {
        public virtual int Id { get; set; }

        public virtual AccountInformation AccountInformation { get; set; }

        [JsonIgnore]
        public virtual string Password { get; set; }

        [JsonIgnore]
        public virtual string PasswordSalt { get; set; }

        public virtual string DisplayName { get; set; }

        public virtual string Email { get; set; }

        public virtual int PermissionLevel { get; set; }

        public virtual int Points { get; set; }
    }
}