using System;
using Newtonsoft.Json;

namespace DomainLayer.Entities
{
    public class AccountInformation
    {
        public AccountInformation()
        {
            DateOfDailyToken = DateTime.Now.AddYears(-20);
            DateOfPrintedDaily = DateTime.Now.AddYears(-20);
        }
        public virtual int Id { get; set; }
        public virtual string NotifyEmail1 { get; set; }
        public virtual string NotifyEmail2 { get; set; }
        public virtual string OfficePhone { get; set; }
        public virtual string OfficeName { get; set; }
        public virtual int NumberOfQuizesTaken { get; set; }
        public virtual int NumberOfPatients { get; set; }
        public bool IsVerified { get; set; }
        public DateTime CreationDate { get; set; }
        [JsonIgnore]
        public virtual long CreditCardToken { get; set; }
        public DateTime DateOfPrintedDaily { get; set; }
        public bool SentDailyPrintout { get; set; }
        public long DailyToken { get; set; }
        public DateTime DateOfDailyToken { get; set; }
        public double Gpa { get; set; }
        public decimal CostPerQuiz  { get; set; }
        public int LastAccountSignedOn { get; set; }
        public bool Autopay { get; set; }
        public int SubscriptionCost { get; set; }
        public DateTime DatePayedThrough { get; set; }
        public bool PayedOnce { get; set; }
    }
}
