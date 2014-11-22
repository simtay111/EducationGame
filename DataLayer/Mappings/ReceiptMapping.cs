using DomainLayer.Entities;
using DomainLayer.Entities.Stories;
using FluentNHibernate.Mapping;

namespace DataLayer.Mappings
{
    public class ReceiptMapping : ClassMap<Receipt>
    {
        public ReceiptMapping()
        {
            Id(x => x.Id);
            Map(x => x.ReceiptText).Length(4000);
            Map(x => x.Cost);
            Map(x => x.Sent);
            Map(x => x.DateBilled);
            References(x => x.AccountInformation).Column("AccountInformationId");
        } 
    }
}