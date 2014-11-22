using DomainLayer.Entities;
using DomainLayer.Entities.Stories;
using FluentNHibernate.Mapping;

namespace DataLayer.Mappings
{
    public class DefaultStoryMapping : ClassMap<DefaultStory>
    {
        public DefaultStoryMapping()
        {
            Id(x => x.Id);
            Map(x => x.Name);
            Map(x => x.StoryOrder);
            Map(x => x.Summary).Length(4000);
            Map(x => x.InfoReferences);
            Map(x => x.MessageLessonText);
        } 
    }
}