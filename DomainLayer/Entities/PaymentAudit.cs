using System;

namespace DomainLayer.Entities
{
    public class PaymentAudit
    {
        public int Id { get; set; }
        public decimal Amount { get; set; }
        public long CreditCardToken { get; set; }
        public AccountInformation AccountInformation  { get; set; }
        public DateTime Created { get; set; }
        public PaymentStatus Status { get; set; }
        public string Message { get; set; }
        public string ItemIds { get; set; }

    }
}