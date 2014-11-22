namespace TangoApi.Entity
{
    public class OrderResponse : BaseOrder
    {
        public string order_id { get; set; }
        public string delivered_at { get; set; }

        public bool success { get; set; }
    }
}