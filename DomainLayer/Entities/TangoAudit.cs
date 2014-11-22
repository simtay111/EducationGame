using System;
using DomainLayer.ECards;

namespace DomainLayer.Entities
{
    public class TangoAudit
    {
        public DateTime TimeStamp { get; set; }
        public TangoCallType CallType { get; set; }
        public string Recipient { get; set; }
        public string ErrorMessage { get; set; }

        public int Id { get; set; }

        public string SKU { get; set; }

        public AccountInformation AccountInformation { get; set; }

        public PaymentStatus Status { get; set; }

        public string AccountIdentifier  { get; set; }

        public string Customer { get; set; }

        public string RecipientName { get; set; }
    }
}