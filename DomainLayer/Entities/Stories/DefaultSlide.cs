
namespace DomainLayer.Entities.Stories
{
    public class DefaultSlide
    {
        public virtual int Id { get; set; }

        public virtual string Body { get; set; }

        public virtual DefaultStory Story { get; set; }

        public virtual string Title { get; set; }
    }
}