using DomainLayer.Entities.Stories;
using FluentNHibernate.Mapping;

namespace DataLayer.Mappings
{
    public class QuestionMapping : ClassMap<Question>
    {
        public QuestionMapping()
        {
            Id(x => x.Id);
            Map(x => x.CorrectAnswer).Length(4000);
            Map(x => x.AnswerBool);
            Map(x => x.OrderToDisplay);
            Map(x => x.Query).Length(4000);
            Map(x => x.WrongAnswer).Length(4000);
            References(x => x.Story).Column("StoryId");
        }
    }
}