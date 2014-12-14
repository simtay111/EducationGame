namespace DomainLayer.Stories.Questions
{
    public class AnswerQuestionRequest
    {
        public int StoryToDoItemId { get; set; }
        public bool Answer { get; set; }
        public int MemberId { get; set; }
    }
}