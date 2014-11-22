using DomainLayer.Entities;
using FluentNHibernate.Mapping;

namespace DataLayer.Mappings
{
    public class ForgotPasswordTokenMapping : ClassMap<ForgotPasswordToken>
    {
        public ForgotPasswordTokenMapping()
        {
            Id(x => x.Id);
            Map(x => x.UniqueToken);
            Map(x => x.Email);
        }
    }
}