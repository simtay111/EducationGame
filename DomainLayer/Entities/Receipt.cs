using System;

namespace DomainLayer.Entities
{
    public class Receipt
    {
        public int Id { get; set; }
        public string ReceiptText { get; set; }
        public decimal Cost { get; set; }
        public bool Sent { get; set; }
        public DateTime DateBilled { get; set; }
        public AccountInformation AccountInformation { get; set; }
    }
}