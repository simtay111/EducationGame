using System;
using DomainLayer.Entities;
using DomainLayer.Entities.Stories;
using DomainLayer.RepoInterfaces;
using DomainLayer.Stories.Responses;
using DomainLayer.Stories.State;

namespace DomainLayer.Stories
{
    public class GetNextStepHandler
    {
        private readonly CurrentStoryStateProvider _stateProvider;
        private readonly StoryStepDataBuilder _dataBuilder;

        public GetNextStepHandler(CurrentStoryStateProvider stateProvider, StoryStepDataBuilder dataBuilder)
        {
            _stateProvider = stateProvider;
            _dataBuilder = dataBuilder;
        }

        public GetNextStepResponse Handle(GetNextStepRequest request)
        {
            var storyToDoItem = _stateProvider.GetNextStep(request.MemberId, request.StoryId);
            return new GetNextStepResponse {Step = _dataBuilder.GenerateFromStoryToDoItem(storyToDoItem)};
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