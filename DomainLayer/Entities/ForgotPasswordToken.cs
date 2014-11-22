
namespace DomainLayer.Entities
{
    public class ForgotPasswordToken
    {
        public virtual int Id { get; set; }
        public virtual string UniqueToken { get; set; }
        public virtual string Email { get; set; }
    }
}