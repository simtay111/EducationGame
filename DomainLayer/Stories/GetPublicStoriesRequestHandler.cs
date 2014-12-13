using System.Collections.Generic;
using DomainLayer.Entities.Stories;
using DomainLayer.RepoInterfaces;

namespace DomainLayer.Stories
{
    public class GetPublicStoriesRequestHandler
    {
        private readonly IStoryRepository _storyRepository;

        public GetPublicStoriesRequestHandler(IStoryRepository storyRepository)
        {
            _storyRepository = storyRepository;
        }

        public GetPublicStoriesResponse Handle(GetPublicStoriesRequest request)
        {
            var publicStories = _storyRepository.GetPublicStories();

            return new GetPublicStoriesResponse{Stories = publicStories};
        }
    }

    public class GetPublicStoriesRequest
    {
        
    }

    public class GetPublicStoriesResponse
    {
        public List<Story> Stories { get; set; }
    }
}