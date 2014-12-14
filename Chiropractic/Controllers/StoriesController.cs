using System.Web.Mvc;
using DataLayer;
using DomainLayer.Stories;
using DomainLayer.Stories.Responses;
using DomainLayer.Stories.State;
using EducationGame.Controllers.CustomResults;

namespace EducationGame.Controllers
{
    public class StoriesController : Controller
    {
        readonly GetPublicStoriesRequestHandler _getPublicStoriesRequestHandler = new GetPublicStoriesRequestHandler(new StoryRepository(new ConnectionProvider()));
        readonly GetStorySummaryHandler _storySummaryHandler = new GetStorySummaryHandler(new StoryRepository(new ConnectionProvider()));
        readonly GetNextStepHandler _getNextStepHandler = new GetNextStepHandler(new CurrentStoryStateProvider(new StoryToDoItemRepository(new ConnectionProvider())), new StoryStepDataBuilder(new SlideRepository(new ConnectionProvider()),new QuestionRepository(new ConnectionProvider()) ));
        readonly StartStoryRequestHandler _startStoryRequestHandler = new StartStoryRequestHandler(new StoryRepository(new ConnectionProvider()), new SlideRepository(new ConnectionProvider()), new QuestionRepository(new ConnectionProvider()), new MemberQuizStatusRepository(new ConnectionProvider()), new MemberRepository(new ConnectionProvider()), new StoryToDoItemRepository(new ConnectionProvider()), new CurrentStoryStateProvider(new StoryToDoItemRepository(new ConnectionProvider())));


        [HttpPost]
        public JsonDotNetResult StartGame(int gameId)
        {
            var memberId = (int)Session[SessionConstants.AccountId];
            var request = new StartStoryRequest { MemberId = memberId, StoryId = gameId };
            var response = _startStoryRequestHandler.Handle(request);

            return new JsonDotNetResult { Data = response.ToDoItem };

        }

        public JsonDotNetResult GetNextSlide(int gameId)
        {
            var memberId = (int)Session[SessionConstants.AccountId];
            var response = _getNextStepHandler.Handle(new GetNextStepRequest { MemberId = memberId, StoryId = gameId });
            return new JsonDotNetResult { Data = response.Step, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        public JsonDotNetResult GetPublicStories()
        {
            var response = _getPublicStoriesRequestHandler.Handle(new GetPublicStoriesRequest());
            return new JsonDotNetResult { Data = response.Stories, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        public JsonDotNetResult GetStorySummary(int gameId)
        {
            var response = _storySummaryHandler.Handle(new GetStorySummaryRequest { GameId = gameId });
            return new JsonDotNetResult { Data = response.Story, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
    }

}
