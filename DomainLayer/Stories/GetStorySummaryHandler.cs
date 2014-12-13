using DomainLayer.Entities.Stories;
using DomainLayer.RepoInterfaces;

namespace DomainLayer.Stories
{
    public class GetStorySummaryHandler
    {
        private readonly IStoryRepository _storyRepository;

        public GetStorySummaryHandler(IStoryRepository storyRepository)
        {
            _storyRepository = storyRepository;
        }

        public GetStorySummaryResponse Handle(GetStorySummaryRequest request)
        {
            return new GetStorySummaryResponse { Story = _storyRepository.GetById(request.GameId) };
        }
    }

    public class GetStorySummaryRequest
    {
        public int GameId { get; set; }
    }

    public class GetStorySummaryResponse
    {
        public Story Story { get; set; }
    }
}