namespace TangoApi.Entity
{
    public class TangoCcInfo
    {
         public TangoBillingInformation billing_address { get; set; }
         public string security_code { get; set; }
         public string expiration { get; set; }
         public string number { get; set; }
    }
}
