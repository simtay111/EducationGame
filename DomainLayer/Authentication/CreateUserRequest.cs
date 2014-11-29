namespace DomainLayer.Authentication
{
    public class CreateUserRequest
    {
        public string UserName { get; set; }

        public string Password { get; set; }

        public string DisplayName { get; set; }

        public string ConfirmPass { get; set; }

        public string CreationAccount { get; set; }

        public int PermissionLevel { get; set; }
    }
}