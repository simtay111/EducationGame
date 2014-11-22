using DomainLayer.Entities;
using FluentNHibernate.Mapping;

namespace DataLayer.Mappings
{
    public class MemberMapping: ClassMap<Member>
    {
        public MemberMapping()
        {
            Id(x => x.Id);
            Map(x => x.FirstName);
            Map(x => x.LastName);
            Map(x => x.PhoneNumber);
            Map(x => x.TotalPoints);
            Map(x => x.Inactive);
            Map(x => x.QuizToken);
            References(x => x.AccountInformation).Column("AccountInformationId");
        } 
    }
}