
namespace DomainLayer.Entities.Stories
{
    public class Slide
    {
        public virtual int Id { get; set; }

        public int OrderToDisplay { get; set; }

        public virtual string Body { get; set; }

        public virtual Story Story { get; set; }

        public virtual string Title { get; set; }
    }
}