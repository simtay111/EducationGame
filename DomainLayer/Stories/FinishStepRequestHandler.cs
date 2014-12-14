using System.Collections.Generic;
using System.Linq;
using DomainLayer.Entities;
using DomainLayer.RepoInterfaces;

namespace DomainLayer.Stories
{
    public class FinishStepRequestHandler
    {
        private readonly IStoryToDoItemRepository _storyToDoItemRepository;

        public FinishStepRequestHandler(IStoryToDoItemRepository storyToDoItemRepository)
        {
            _storyToDoItemRepository = storyToDoItemRepository;
        }

        public FinishStepResponse Handle(FinishStepRequest request)
        {
            var existingItems = _storyToDoItemRepository.GetByMemberId(request.MemberId);
            if (DoesTheIdBelongToTheMember(request, existingItems))
                _storyToDoItemRepository.Delete(request.StoryToDoItemId);

            return new FinishStepResponse();
        }

        private static bool DoesTheIdBelongToTheMember(FinishStepRequest request, List<StoryToDoItem> existingItems)
        {
            return existingItems.Any(x => x.Id == request.StoryToDoItemId);
        }
    }

    public class FinishStepRequest
    {
        public int StoryToDoItemId { get; set; }
        public int MemberId { get; set; }
    }

    public class FinishStepResponse
    {

    }
}