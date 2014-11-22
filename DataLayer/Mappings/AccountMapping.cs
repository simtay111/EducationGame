using DomainLayer.Entities;
using FluentNHibernate.Mapping;

namespace DataLayer.Mappings
{
    public class AccountMapping : ClassMap<Account>
    {
        public AccountMapping()
        {
            Id(x => x.Id);
            Map(x => x.DisplayName);
            Map(x => x.Email);
            Map(x => x.Password);
            Map(x => x.PasswordSalt);
            Map(x => x.PermissionLevel);
            Map(x => x.Points);
            References(x => x.AccountInformation).Column("AccountInformationId");
        }
    }
}