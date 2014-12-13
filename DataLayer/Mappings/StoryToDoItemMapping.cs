using DomainLayer.Entities;
using FluentNHibernate.Mapping;

namespace DataLayer.Mappings
{
    public class StoryToDoItemMapping : ClassMap<StoryToDoItem>
    {
        public StoryToDoItemMapping()
        {
            Id(x => x.Id);
            Map(x => x.Type);
            Map(x => x.ToDoId);
            References(x => x.Member).Column("MemberId");
            References(x => x.Story).Column("StoryId");
        }
    }
}