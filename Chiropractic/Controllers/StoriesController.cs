using System.Web.Mvc;
using DataLayer;
using DomainLayer.Stories;
using DomainLayer.Stories.Questions;
using DomainLayer.Stories.Responses;
using DomainLayer.Stories.State;
using EducationGame.Controllers.CustomResults;

namespace EducationGame.Controllers
{
    public class StoriesController : Controller
    {
        readonly GetPublicStoriesRequestHandler _getPublicStoriesRequestHandler = new GetPublicStoriesRequestHandler(new StoryRepository(new ConnectionProvider()));
        readonly GetStorySummaryHandler _storySummaryHandler = new GetStorySummaryHandler(new StoryRepository(new ConnectionProvider()));
        readonly GetNextStepHandler _getNextStepHandler = new GetNextStepHandler(new CurrentStoryStateProvider(new StoryToDoItemRepository(new ConnectionProvider())), new StoryStepDataBuilder(new SlideRepository(new ConnectionProvider()), new QuestionRepository(new ConnectionProvider())));
        readonly StartStoryRequestHandler _startStoryRequestHandler = new StartStoryRequestHandler(new StoryRepository(new ConnectionProvider()), new SlideRepository(new ConnectionProvider()), new QuestionRepository(new ConnectionProvider()), new MemberQuizStatusRepository(new ConnectionProvider()), new MemberRepository(new ConnectionProvider()), new StoryToDoItemRepository(new ConnectionProvider()), new CurrentStoryStateProvider(new StoryToDoItemRepository(new ConnectionProvider())));
        readonly FinishStepRequestHandler _finishStepRequestHandler = new FinishStepRequestHandler(new StoryToDoItemRepository(new ConnectionProvider()));
        readonly AnswerQuestionHandler _answerQuestionHandler = new AnswerQuestionHandler(new QuestionRepository(new ConnectionProvider()), new StoryToDoItemRepository(new ConnectionProvider()), new PointsWithCompanyRepository(new ConnectionProvider()), new MemberRepository(new ConnectionProvider()));
        readonly GetStoriesForAcctRequestHandler _getStoriesForAcctRequestHandler = new GetStoriesForAcctRequestHandler(new StoryRepository(new ConnectionProvider()));
        readonly GetEntireStoryRequestHandler _getEntireStoryRequestHandler = new GetEntireStoryRequestHandler(new StoryRepository(new ConnectionProvider()), new SlideRepository(new ConnectionProvider()), new QuestionRepository(new ConnectionProvider()));

        [Authorize]
        public JsonDotNetResult GetEntireStory(int storyId)
        {
            var response = _getEntireStoryRequestHandler.Handle(new GetEntireStoryRequest {StoryId = storyId});
            return new JsonDotNetResult{Data = response, JsonRequestBehavior = JsonRequestBehavior.AllowGet};
        }

        [Authorize]
        [HttpPost]
        public JsonDotNetResult StartGame(int gameId)
        {
            var memberId = (int)Session[SessionConstants.AccountId];
            var request = new StartStoryRequest { MemberId = memberId, StoryId = gameId };
            var response = _startStoryRequestHandler.Handle(request);

            return new JsonDotNetResult { Data = response.ToDoItem };

        }

        [HttpPost]
        [Authorize]
        public void FinishStep(int stepId)
        {
            var memberId = (int)Session[SessionConstants.AccountId];
            var request = new FinishStepRequest { StoryToDoItemId = stepId, MemberId = memberId };
            _finishStepRequestHandler.Handle(request);
        }

        [HttpPost]
        [Authorize]
        public JsonDotNetResult AnswerQuestion(int stepId, bool answer)
        {
            var memberId = (int)Session[SessionConstants.AccountId];
            var request = new AnswerQuestionRequest
            {
                Answer = answer,
                MemberId = memberId,
                StoryToDoItemId = stepId
            };

            try
            {
                _answerQuestionHandler.Handle(request);
            }
            catch (AnswerQuestionException ex)
            {
                return new JsonDotNetResult();
            }
            return new JsonDotNetResult();
        }

        [Authorize]
        [HttpPost]
        public JsonDotNetResult FinishGame(int gameId)
        {
            var handler = new FinishGameHandler(new MemberQuizStatusRepository(new ConnectionProvider()),
                new StoryToDoItemRepository(new ConnectionProvider()), new MemberRepository(new ConnectionProvider()),
                new StoryRepository(new ConnectionProvider()), new PointsWithCompanyRepository(new ConnectionProvider()));

            var memberId = (int)Session[SessionConstants.AccountId];
            var request = new FinishGameRequest { MemberId = memberId, GameId = gameId };
            var response = handler.Handle(request);

            return new JsonDotNetResult { Data = response };
        }

        [Authorize]
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

        [Authorize]
        public JsonDotNetResult GetStoriesForAccount()
        {
            var response = _getStoriesForAcctRequestHandler.Handle(new GetStoriesForAcctRequest { AccountId = SessionConstants.GetAccountInfoId((int)Session[SessionConstants.AccountId]) });
            return new JsonDotNetResult { Data = response.Stories, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        public JsonDotNetResult GetStorySummary(int gameId)
        {
            var response = _storySummaryHandler.Handle(new GetStorySummaryRequest { GameId = gameId });
            return new JsonDotNetResult { Data = response.Story, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
    }

}
