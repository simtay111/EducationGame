
namespace DomainLayer.Entities.Stories
{
    public class DefaultQuestion
    {
        public virtual int Id { get; set; }
        public virtual int OrderToDisplay { get; set; }
        public virtual string Query { get; set; }
        public virtual bool AnswerBool { get; set; }
        public virtual string CorrectAnswer { get; set; }
        public virtual string WrongAnswer { get; set; }
        public virtual DefaultStory Story { get; set; }
    }
}