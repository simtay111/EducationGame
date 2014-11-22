namespace DomainLayer.Entities
{
    public interface IHaveAuthorizationCredentials
    {
        string Password { get; set; }
        string PasswordSalt { get; set; }
        string Email { get; set; }
    }
}