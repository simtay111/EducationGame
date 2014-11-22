using DomainLayer.Entities;
using FluentNHibernate.Mapping;

namespace DataLayer.Mappings
{
    public class PaymentAuditMapping : ClassMap<PaymentAudit>
    {
        public PaymentAuditMapping()
        {
            Id(x => x.Id);
            Map(x => x.Created);
            Map(x => x.ItemIds);
            Map(x => x.Amount);
            Map(x => x.CreditCardToken);
            Map(x => x.Message);
            Map(x => x.Status);
            References(x => x.AccountInformation).Column("AccountInformationId");
        } 
    }
}