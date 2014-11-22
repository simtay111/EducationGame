using DomainLayer.Entities.Stories;
using FluentNHibernate.Mapping;

namespace DataLayer.Mappings
{
    public class DefaultSlideMapping : ClassMap<DefaultSlide>
    {
        public DefaultSlideMapping()
        {
            Id(x => x.Id);
            Map(x => x.Body).Length(4000);
            Map(x => x.Title);
            References(x => x.Story).Column("DefaultStoryId");
        } 
    }
}