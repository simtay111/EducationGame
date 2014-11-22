using TangoApi.Entity;

namespace TangoApi
{
    public class CreateAccountRequest
    {
        public string customer { get; set; }
        public string email { get; set; }
        public string identifier { get; set; }
        
    }
    public class CreateAccountResponse
    {
        public bool success { get; set; }
        public TangoAccount account { get; set; }
    }

    public class AccountCreator
    {
        private readonly ServiceProxy _proxy;

        public AccountCreator(ServiceProxy proxy)
        {
            _proxy = proxy;
        }

        public void CreateAccount(CreateAccountRequest request)
        {
            var response = _proxy.Execute<CreateAccountRequest, CreateAccountResponse>(request,
                                                                        TangoCredentials.Endpoint +
                                                                            "/accounts");


        }
    }
}