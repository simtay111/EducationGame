namespace DomainLayer.Entities
{
    public interface IHaveAuthorizationCredentials
    {
        int Id { get; set; }
        string Password { get; set; }
        string PasswordSalt { get; set; }
        string Email { get; set; }
    }
}