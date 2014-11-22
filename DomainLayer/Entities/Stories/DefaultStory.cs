namespace DomainLayer.Entities.Stories
{
    public class DefaultStory 
    {
        public virtual int Id { get; set; }
        public virtual string Name { get; set; }
        public virtual int StoryOrder { get; set; }
        public virtual string Summary { get; set; }
        public virtual string InfoReferences { get; set; }
        public string MessageLessonText { get; set; } 
    }
}