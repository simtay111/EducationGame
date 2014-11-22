
using System;

namespace DomainLayer.Entities
{
    public class AwardedPrize
    {
        public AwardedPrize()
        {
            DateBilledToOffice = DateTime.Now;
        }
        public virtual int Id { get; set; }
        public virtual string ConfirmationCode { get; set; }
        public virtual DateTime IssueDate { get; set; }
        public virtual bool Redeemed { get; set; }
        public virtual string PrizeName { get; set; }
        public virtual Member Member { get; set; }
        public virtual bool Ordered { get; set; }
        public virtual DateTime DateOrdered { get; set; }
        public virtual bool BilledToOffice { get; set; }
        public virtual DateTime DateBilledToOffice { get; set; }
        public virtual AccountInformation AccountInformation { get; set; }
        public string PrizeSku { get; set; }
        public int PrizePoints { get; set; }
    }

}