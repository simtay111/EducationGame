using DomainLayer.Entities;
using FluentNHibernate.Mapping;

namespace DataLayer.Mappings
{
    public class StoryBlackListMapping : ClassMap<StoryBlackList>
    {
        public StoryBlackListMapping()
        {
            Id(x => x.Id);
            References(x => x.Story).Column("StoryId");
            References(x => x.AccountInformation).Column("AccountInformationId");
        } 
    }
}