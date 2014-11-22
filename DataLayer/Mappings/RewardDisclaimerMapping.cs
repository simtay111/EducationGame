using DomainLayer.Entities;
using FluentNHibernate.Mapping;

namespace DataLayer.Mappings
{
    public class RewardDisclaimerMapping : ClassMap<RewardDisclaimer>
    {
        public RewardDisclaimerMapping()
        {
            Id(x => x.Id);
            Map(x => x.Description).Length(10000);
            Map(x => x.Disclaimer).Length(10000);
            Map(x => x.TermsAndConditions).Length(10000);
            Map(x => x.Sku);
        }
    }
}