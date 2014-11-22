namespace TangoApi.Entity
{
    public class Reward
    {
         
        public string description { get; set; } 
        public string sku { get; set; } 
        public string currency_type { get; set; } 
        public long unit_price { get; set; } 
        public string min_price { get; set; } 
        public string max_price { get; set; } 
        public bool available { get; set; } 
    }
}