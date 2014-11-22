using DomainLayer.Entities;
using FluentNHibernate.Mapping;

namespace DataLayer.Mappings
{
    public class TangoAuditMapping : ClassMap<TangoAudit>
    {
        public TangoAuditMapping()
        {
            Id(x => x.Id);
            Map(x => x.TimeStamp);
            Map(x => x.ErrorMessage);
            Map(x => x.Recipient);
            Map(x => x.CallType);
            Map(x => x.SKU);
            Map(x => x.Status);
            Map(x => x.RecipientName);
            Map(x => x.AccountIdentifier);
            Map(x => x.Customer);
            References(x => x.AccountInformation).Column("AccountInformationId");
        } 
    }
}