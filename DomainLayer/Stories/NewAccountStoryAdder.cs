using System.Linq;
using DomainLayer.Entities;
using DomainLayer.Entities.Stories;
using DomainLayer.RepoInterfaces;

namespace DomainLayer.Stories
{
    public interface INewAccountStoryAdder
    {
        void AddStoriesToAccount(AccountInformation acct);
    }

    public class NewAccountStoryAdder : INewAccountStoryAdder
    {
        private readonly ISlideRepository _slideRepository;
        private readonly IStoryRepository _storyRepository;
        private readonly IQuestionRepository _questionRepository;
        private readonly IDefaultQuestionRepository _defaultQuestionRepository;
        private readonly IDefaultSlideRepository _defaultSlideRepository;
        private readonly IDefaultStoryRepository _defaultStoryRepository;

        public NewAccountStoryAdder(ISlideRepository slideRepository, IStoryRepository storyRepository, IQuestionRepository questionRepository, IDefaultQuestionRepository defaultQuestionRepository, IDefaultSlideRepository defaultSlideRepository, IDefaultStoryRepository defaultStoryRepository)
        {
            _slideRepository = slideRepository;
            _storyRepository = storyRepository;
            _questionRepository = questionRepository;
            _defaultQuestionRepository = defaultQuestionRepository;
            _defaultSlideRepository = defaultSlideRepository;
            _defaultStoryRepository = defaultStoryRepository;
        }


        public void AddStoriesToAccount(AccountInformation acct)
        {
            var defaultStories = _defaultStoryRepository.GetAll();
            var defaultSlides = _defaultSlideRepository.GetAll();
            var defaultQuestions = _defaultQuestionRepository.GetAll();

            foreach (var story in defaultStories)
            {
                var newStory = new Story
                    {
                        AccountInformation = acct,
                        StoryOrder = story.StoryOrder,
                        Summary = story.Summary,
                        InfoReferences = story.InfoReferences,
                        MessageLessonText = story.MessageLessonText,
                        Name = story.Name,
                    };
                _storyRepository.Save(newStory);
                foreach (var slide in defaultSlides.Where(x => x.Story.Id == story.Id))
                {
                    var newSlide = new Slide
                        {
                            Body = slide.Body,
                            Story = newStory,
                            Title = slide.Title
                        };
                    _slideRepository.Save(newSlide);
                }
                foreach (var question in defaultQuestions.Where(x => x.Story.Id == story.Id))
                {
                    var newQuestion = new Question
                        {
                            AnswerBool = question.AnswerBool,
                            CorrectAnswer = question.CorrectAnswer,
                            OrderToDisplay = question.OrderToDisplay,
                            Query = question.Query,
                            Story = newStory, 
                            WrongAnswer = question.WrongAnswer
                        };

                    _questionRepository.Save(newQuestion);
                }
            }
        }
    }
}