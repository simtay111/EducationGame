using System.Collections.Generic;
using System.Linq;
using TangoApi.Entity;

namespace TangoApi
{
    public class AvailableItemResponse
    {
        public bool success { get; set; }
        public List<AvailableItem> brands { get; set; }

    }

    public class AvailableItemRequest
    {
        
    }
    public class AvailableItemFetcher
    {
        private readonly ServiceProxy _serviceProxy;

        public AvailableItemFetcher(ServiceProxy serviceProxy)
        {
            _serviceProxy = serviceProxy;
        }

        public List<AvailableItem> Fetch()
        {
            var request = new AvailableItemRequest();
            string uriString = TangoCredentials.Endpoint + "/rewards";
            var response = _serviceProxy.Execute<AvailableItemRequest, AvailableItemResponse>(request, uriString);
            
            foreach (var reward in response.brands)
            {
                reward.rewards = reward.rewards.Where(x => x.unit_price == 500 || x.unit_price == 1000 || x.unit_price == 2000 || x.unit_price == 2500 || (x.unit_price == -1 && int.Parse(x.min_price) < 500 && int.Parse(x.max_price)
                     > 500)).ToList();
            }

            return response.brands.Where(x => x.rewards.Count > 0).ToList();
        }
    }
}