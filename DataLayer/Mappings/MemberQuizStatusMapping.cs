using DomainLayer.Entities.Quizes;
using FluentNHibernate.Mapping;

namespace DataLayer.Mappings
{
    public class MemberQuizStatusMapping : ClassMap<MemberQuizStatus>
    {
        public MemberQuizStatusMapping()
        {
            Id(x => x.Id);
            Map(x => x.DateCompleted);
            Map(x => x.PointsEarned);
            Map(x => x.StoryName);
            Map(x => x.Completed);
            Map(x => x.StoryId);
            References(x => x.Member).Column("MemberId");
        } 
    }
}