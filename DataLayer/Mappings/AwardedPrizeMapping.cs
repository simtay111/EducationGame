using DomainLayer.Entities;
using FluentNHibernate.Mapping;

namespace DataLayer.Mappings
{
    public class AwardedPrizeMapping: ClassMap<AwardedPrize>
    {
       public AwardedPrizeMapping()
       {
           Id(x => x.Id);
           Map(x => x.ConfirmationCode);
           Map(x => x.IssueDate);
           Map(x => x.PrizeName);
           Map(x => x.Redeemed);
           Map(x => x.Ordered);
           Map(x => x.BilledToOffice);
           Map(x => x.DateBilledToOffice);
           Map(x => x.DateOrdered);
           Map(x => x.PrizePoints);
           Map(x => x.PrizeSku);
           References(x => x.Member).Column("MemberId");
           References(x => x.AccountInformation).Column("AccountInformationId");
       } 
    }
}