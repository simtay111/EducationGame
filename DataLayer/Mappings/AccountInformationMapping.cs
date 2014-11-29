using DomainLayer.Entities;
using FluentNHibernate.Mapping;

namespace DataLayer.Mappings
{
    public class AccountInformationMapping : ClassMap<AccountInformation>
    {
        public AccountInformationMapping()
        {
            Id(x => x.Id);
            Map(x => x.PayedOnce);
            Map(x => x.CreationDate);
            Map(x => x.CreditCardToken);
            Map(x => x.Autopay);
            Map(x => x.DatePayedThrough);
            Map(x => x.SubscriptionCost);
        }
    }
}