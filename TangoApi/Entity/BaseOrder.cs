namespace TangoApi.Entity
{
    public class BaseOrder
    {
        public string account_identifier { get; set; } 
        public string sku { get; set; } 
        public string reward_message { get; set; } 
        public string reward_subject { get; set; } 
        public string reward_from { get; set; }
        public Recipient recipient { get; set; }
    }
}