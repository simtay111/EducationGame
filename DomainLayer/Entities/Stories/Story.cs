using System.Collections.Generic;

namespace DomainLayer.Entities.Stories
{
    public class Story 
    {
        public virtual int Id { get; set; }
        public virtual string Name { get; set; }
        public virtual int StoryOrder { get; set; }
        public virtual string Summary { get; set; }
        public string MessageLessonText { get; set; }
        public AccountInformation AccountInformation { get; set; }
    }
}
