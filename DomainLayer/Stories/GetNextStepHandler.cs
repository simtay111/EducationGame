using System;
using DomainLayer.Entities.Stories;
using DomainLayer.RepoInterfaces;
using DomainLayer.Stories.State;

namespace DomainLayer.Stories
{
    public class GetNextStepHandler
    {
        private readonly CurrentStoryStateProvider _stateProvider;
        public GetNextStepHandler(CurrentStoryStateProvider stateProvider)
        {
            _stateProvider = stateProvider;
        }

        public GetNextStepResponse Handle(GetNextStepRequest request)
        {
            return new GetNextStepResponse {Step = _stateProvider.GetNextStep(request.MemberId, request.StoryId)};
        }
    }

    public class GetNextStepResponse
    {
        public Object Step { get; set; }
    }

    public class GetNextStepRequest
    {
        public int StoryId { get; set; }
        public int MemberId { get; set; }
    }
}