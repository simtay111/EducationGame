using TangoApi.Entity;

namespace TangoApi
{


    public class OrderPlacer
    {
        private readonly ServiceProxy _serviceProxy;

        public OrderPlacer(ServiceProxy serviceProxy)
        {
            _serviceProxy = serviceProxy;
        }

        public OrderResponse Order(OrderWithAmountRequest order)
        {
            var response = _serviceProxy.Execute<OrderWithAmountRequest, OrderResponse>(order, TangoCredentials.Endpoint + "/orders");

            return response;
        }

        public OrderResponse Order(OrderRequest order)
        {
            var response = _serviceProxy.Execute<OrderRequest, OrderResponse>(order, TangoCredentials.Endpoint + "/orders");

            return response;
        }
    }
}
