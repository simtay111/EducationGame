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
            Map(x => x.Inactive);
            Map(x => x.Email);
            Map(x => x.Password);
            Map(x => x.PasswordSalt);
        } 
    }
}