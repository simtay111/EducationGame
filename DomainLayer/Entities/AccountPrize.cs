
using System;

namespace DomainLayer.Entities
{
    public class AccountPrize
    {
        public AccountPrize()
        {
            IssueDate = DateTime.Now;
        }
        public virtual int Id { get; set; }
        public virtual DateTime IssueDate { get; set; }
        public virtual string PrizeName { get; set; }
        public virtual AccountInformation AccountInformation { get; set; }
        public string PrizeSku { get; set; }
        public int PrizePoints { get; set; }
        public string TargetEmail { get; set; }
        public int AccountId { get; set; }
    }
}