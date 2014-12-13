using System;

namespace DomainLayer.Entities.Quizes
{
    public class MemberQuizStatus
    {
        public virtual int Id { get; set; }

        public virtual Member Member { get; set; }

        public virtual string StoryName { get; set; }

        public virtual int PointsEarned { get; set; }

        public virtual int StoryId { get; set; }

        public virtual bool Completed { get; set; }

        public virtual DateTime DateCompleted { get; set; }
    }
}