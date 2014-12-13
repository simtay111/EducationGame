using System.Collections.Generic;
using System.Linq;
using DomainLayer.Constants;
using DomainLayer.Entities;
using DomainLayer.Entities.Quizes;
using DomainLayer.RepoInterfaces;
using DomainLayer.Stories.Pocos;
using DomainLayer.Stories.State;

namespace DomainLayer.Stories
{
    public class StartStoryRequestHandler
    {
        private readonly IStoryRepository _storyRepository;
        private readonly ISlideRepository _slideRepository;
        private readonly IQuestionRepository _questionRepository;
        private readonly IMemberQuizStatusRepository _memberQuizStatusRepository;
        private readonly IMemberRepository _memberRepository;
        private readonly IStoryToDoItemRepository _storyToDoItemRepository;
        private readonly CurrentStoryStateProvider _currentStoryStateProvider;

        public StartStoryRequestHandler(IStoryRepository storyRepository, ISlideRepository slideRepository, IQuestionRepository questionRepository, IMemberQuizStatusRepository memberQuizStatusRepository, IMemberRepository memberRepository, IStoryToDoItemRepository storyToDoItemRepository, CurrentStoryStateProvider currentStoryStateProvider)
        {
            _storyRepository = storyRepository;
            _slideRepository = slideRepository;
            _questionRepository = questionRepository;
            _memberQuizStatusRepository = memberQuizStatusRepository;
            _memberRepository = memberRepository;
            _storyToDoItemRepository = storyToDoItemRepository;
            _currentStoryStateProvider = currentStoryStateProvider;
        }

        public StartStoryResponse Handle(StartStoryRequest request)
        {
            var story = _storyRepository.GetById(request.StoryId);
            var slides = _slideRepository.GetForStory(request.StoryId);
            var questions = _questionRepository.GetForStory(request.StoryId);
            var member = _memberRepository.GetById(request.MemberId);

            var existingStory = _memberQuizStatusRepository.GetByStoryIdAndMemberId(request.StoryId, request.MemberId);
            if (existingStory == null)
            {
                var newStatus = new MemberQuizStatus
                {
                    Member = member,
                    StoryId = request.StoryId,
                    StoryName = story.Name
                };

                _memberQuizStatusRepository.Save(newStatus);
                foreach (var slide in slides)
                {
                    var storyToDoItem = new StoryToDoItem
                    {
                        Member = member,
                        Story = story,
                        ToDoId = slide.Id,
                        Type = ToDoType.Slide
                    };
                    _storyToDoItemRepository.Save(storyToDoItem);
                }
                foreach (var question in questions)
                {
                    var storyToDoItem = new StoryToDoItem
                    {
                        Member = member,
                        Story = story,
                        ToDoId = question.Id,
                        Type = ToDoType.Question
                    };
                    _storyToDoItemRepository.Save(storyToDoItem);
                }
            }
            var currentStoryState = _currentStoryStateProvider.GetNextStep(request.MemberId, request.StoryId);

            return new StartStoryResponse {ToDoItem = currentStoryState};
        }
    }

    public class StartStoryRequest
    {
        public int StoryId { get; set; }
        public int MemberId { get; set; }
    }

    public class StartStoryResponse
    {
        public StoryToDoItem ToDoItem { get; set; }
    }
}