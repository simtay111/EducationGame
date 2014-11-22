using DomainLayer.Entities.Stories;

namespace DomainLayer.Entities
{
    public class StoryBlackList
    {
        public virtual int Id { get; set; }
        public virtual AccountInformation AccountInformation { get; set; }
        public virtual Story Story { get; set; }
    }
}