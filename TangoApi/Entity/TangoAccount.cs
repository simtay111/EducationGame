namespace TangoApi.Entity
{
    public class TangoAccount
    {
        public string identifier { get; set; } 
        public string email { get; set; } 
        public string customer { get; set; } 
        public long available_balance { get; set; } 
    }
}