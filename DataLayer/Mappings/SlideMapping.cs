using DomainLayer.Entities.Stories;
using FluentNHibernate.Mapping;

namespace DataLayer.Mappings
{
    public class SlideMapping : ClassMap<Slide>
    {
        public SlideMapping()
        {
            Id(x => x.Id);
            Map(x => x.Body).Length(4000);
            Map(x => x.Title);
            Map(x => x.OrderToDisplay);
            References(x => x.Story).Column("StoryId");
        } 
    }
}