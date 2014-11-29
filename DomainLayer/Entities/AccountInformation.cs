using System;
using Newtonsoft.Json;

namespace DomainLayer.Entities
{
    public class AccountInformation
    {
        public virtual int Id { get; set; }
        public virtual string CompanyName { get; set; }
        public DateTime CreationDate { get; set; }
        [JsonIgnore]
        public virtual long CreditCardToken { get; set; }
        public bool Autopay { get; set; }
        public int SubscriptionCost { get; set; }
        public DateTime DatePayedThrough { get; set; }
        public bool PayedOnce { get; set; }
    }
}
