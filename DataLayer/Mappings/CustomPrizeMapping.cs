using DomainLayer.Entities;
using FluentNHibernate.Mapping;

namespace DataLayer.Mappings
{
    public class CustomPrizeMapping: ClassMap<CustomPrize>
    {
       public CustomPrizeMapping()
       {
           Id(x => x.Id);
           Map(x => x.Points);
           Map(x => x.Name);
           References(x => x.AccountInformation).Column("AccountInformationId");
       } 
    }
}