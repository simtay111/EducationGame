using TangoApi.Entity;

namespace TangoApi
{
    public class AccountStatusRequest
    {
        
    }

    public class AccountStatusResponse
    {
        public string success { get; set; }
        public TangoAccount account { get; set; }
    }
    public class AccountStatusFetcher
    {
        private readonly ServiceProxy _proxy;

        public AccountStatusFetcher(ServiceProxy proxy)
        {
            _proxy = proxy;
        }

        public AccountStatusResponse GetCurrentStatus(string customer, string accountIdent)
        {
            return _proxy.Execute<AccountStatusRequest, AccountStatusResponse>(new AccountStatusRequest(), string.Format(TangoCredentials.Endpoint + "/accounts/{0}/{1}", customer, accountIdent));
        }
    }
}