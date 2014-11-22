using TangoApi.Entity;

namespace TangoApi
{
    public class AddFundsRequest
    {
        public string customer { get; set; }
        public string account_identifier { get; set; }
        public long amount { get; set; }
        public string client_ip { get; set; }
        public TangoCcInfo credit_card { get; set; }
    }

    public class AddFundsResponse
    {
        public bool success { get; set; } 
        public string fund_id { get; set; } 
        public long amount { get; set; } 
        
    }
    public class FundsAdder
    {
        private readonly ServiceProxy _serviceProxy;

        public FundsAdder(ServiceProxy serviceProxy)
        {
            _serviceProxy = serviceProxy;
        }

        public void AddFunds(AddFundsRequest request)
        {
            _serviceProxy.Execute<AddFundsRequest, AddFundsResponse>(request,
                                                                     TangoCredentials.Endpoint + "/funds");
        }
    }
}