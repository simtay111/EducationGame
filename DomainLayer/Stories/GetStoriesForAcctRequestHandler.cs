using System.Collections.Generic;
using DomainLayer.Entities.Stories;
using DomainLayer.RepoInterfaces;

namespace DomainLayer.Stories
{
    public class GetStoriesForAcctRequestHandler
    {
        private readonly IStoryRepository _storyRepository;

        public GetStoriesForAcctRequestHandler(IStoryRepository storyRepository)
        {
            _storyRepository = storyRepository;
        }

        public GetStoriesForAcctResponse Handle(GetStoriesForAcctRequest request)
        {
            var stories = _storyRepository.GetAllForAcctInfo(request.AccountId);

            return new GetStoriesForAcctResponse {Stories = stories};
        }
         
    }

    public class GetStoriesForAcctRequest
    {
        public int AccountId { get; set; }
    }
    public class GetStoriesForAcctResponse
    {
        public List<Story> Stories { get; set; }
    }
}