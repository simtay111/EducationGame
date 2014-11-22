using DomainLayer.Entities;
using FluentNHibernate.Mapping;

namespace DataLayer.Mappings
{
    public class AvailablePrizeMapping : ClassMap<AvailablePrize>
    {
        public AvailablePrizeMapping()
        {
            Id(x => x.Id);
            Map(x => x.Points);
            Map(x => x.Name);
            Map(x => x.Sku);
            Map(x => x.ImageUrl);
            Map(x => x.Cost);
            Map(x => x.IsRange);
        }
    }
}