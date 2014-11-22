using DomainLayer.Entities;
using FluentNHibernate.Mapping;

namespace DataLayer.Mappings
{
    public class AccountPrizeMapping: ClassMap<AccountPrize>
    {
       public AccountPrizeMapping()
       {
           Id(x => x.Id);
           Map(x => x.IssueDate);
           Map(x => x.PrizeName);
           Map(x => x.PrizePoints);
           Map(x => x.PrizeSku);
           Map(x => x.TargetEmail);
           Map(x => x.AccountId);
           References(x => x.AccountInformation).Column("AccountInformationId");
       } 
    }
}