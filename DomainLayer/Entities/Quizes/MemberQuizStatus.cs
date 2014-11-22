using System;

namespace DomainLayer.Entities.Quizes
{
    public class MemberQuizStatus
    {
        public MemberQuizStatus()
        {
            DatePayedFor = DateTime.Now;
        }
        public virtual int Id { get; set; }

        public virtual Member Member { get; set; }

        public virtual int GeneratedToken { get; set; }

        public virtual string StoryName { get; set; }

        public virtual int PointsEarned { get; set; }

        public virtual int StoryId { get; set; }
        public virtual bool PayedFor { get; set; }
        public virtual decimal PayedAmount { get; set; }
        public virtual DateTime DatePayedFor { get; set; }

        public virtual bool Completed { get; set; }

        public virtual DateTime DateCompleted { get; set; }
    }
}