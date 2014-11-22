using System;
using System.Collections.Generic;
using TangoApi.Entity;

namespace TangoApi
{
    public class OrderHistoryRequest
    {

    }

    public class OrderHistoryRequestData
    {
        public string account_identifier { get; set; }
        public string customer { get; set; }
        public int offset { get; set; }
        public int limit { get; set; }
        public DateTime start_date { get; set; }
        public DateTime end_date { get; set; }
    }

    public class OrderHistory
    {
        public string order_id { get; set; }
        public string account_identifier { get; set; }
        public string sku { get; set; }
        public int amount { get; set; }
        public string reward_message { get; set; }
        public string reward_subject { get; set; }
        public DateTime delivered_at { get; set; }
        public Recipient recipient { get; set; }

    }

    public class OrderHistoryResponse
    {
        public string success { get; set; }
        public List<OrderHistory> orders { get; set; }
    }
    public class OrderHistoryFetcher
    {
        private readonly ServiceProxy _proxy;

        public OrderHistoryFetcher(ServiceProxy proxy)
        {
            _proxy = proxy;
        }

        public OrderHistoryResponse GetCurrentStatus(OrderHistoryRequestData request)
        {
            return _proxy.Execute<OrderHistoryRequest, OrderHistoryResponse>(new OrderHistoryRequest(),
                string.Format(TangoCredentials.Endpoint + "/orders?customer={0}&account_identifier={1}&offset={2}&limit={3}&start_date={4}&end_date={5}", request.customer, request.account_identifier, request.offset, request.limit, request.start_date.ToString(@"yyyy-MM-ddTHH\:mm\:ss.fffffffzzz"), request.end_date.ToString(@"yyyy-MM-ddTHH\:mm\:ss.fffffffzzz")));
        }
    }
}