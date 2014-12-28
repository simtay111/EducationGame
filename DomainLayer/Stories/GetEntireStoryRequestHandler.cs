using System.Collections.Generic;
using DomainLayer.Entities.Stories;
using DomainLayer.RepoInterfaces;

namespace DomainLayer.Stories
{
    public class GetEntireStoryRequestHandler
    {
        private readonly IStoryRepository _storyRepository;
        private readonly ISlideRepository _slideRepository;
        private readonly IQuestionRepository _questionRepository;

        public GetEntireStoryRequestHandler(IStoryRepository storyRepository, ISlideRepository slideRepository, IQuestionRepository questionRepository)
        {
            _storyRepository = storyRepository;
            _slideRepository = slideRepository;
            _questionRepository = questionRepository;
        }

        public GetEntireStoryResponse Handle(GetEntireStoryRequest request)
        {
            return new GetEntireStoryResponse
            {
                Story = _storyRepository.GetById(request.StoryId),
                Slides = _slideRepository.GetForStory(request.StoryId),
                Questions = _questionRepository.GetForStory(request.StoryId)
            };
        }
    }

    public class GetEntireStoryRequest
    {
        public int StoryId { get; set; }
    }

    public class GetEntireStoryResponse
    {
        public Story Story { get; set; }
        public List<Slide> Slides { get; set; }
        public List<Question> Questions { get; set; }
    }
}