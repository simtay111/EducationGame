using DomainLayer.Entities.Quizes;
using FluentNHibernate.Mapping;

namespace DataLayer.Mappings
{
    public class PointsWithCompanyMapping : ClassMap<PointsWithCompany>
    {
        public PointsWithCompanyMapping()
        {
            Id(x => x.Id);
            References(x => x.AccountInformation).Column("AccountInformationId");
            References(x => x.Member).Column("MemberId");
            Map(x => x.Points);
        } 
    }
}