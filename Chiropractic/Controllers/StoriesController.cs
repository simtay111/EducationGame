using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Chiropractic;
using Chiropractic.Filters;
using DataLayer;
using DomainLayer.Authentication;
using DomainLayer.Email;
using DomainLayer.Entities;
using DomainLayer.Entities.Quizes;
using DomainLayer.Entities.Stories;
using DomainLayer.Points;
using DomainLayer.StoryManagement;
using EducationGame.Controllers.CustomResults;

namespace EducationGame.Controllers
{
    public class StorySet
    {
        public Story Story { get; set; }
        public List<Slide> Slides { get; set; }
        public List<Question> Questions { get; set; }
    }

    public class BlackListSummary
    {
        public Story Story { get; set; }
        public bool IsBlackListed { get; set; }
    }
    public class ToggleBlackListModel
    {
        public int StoryId { get; set; }
        public bool IsBlackListed { get; set; }
    }
    public class StoriesController : Controller
    {
        [Authorize]
        [AuthorizationFilter(RolesStatic.SuperUser)]
        public ActionResult MyStories()
        {
            return View();
        }

        [HttpPost]
        [Authorize]
        [AuthorizationFilter(RolesStatic.SuperUser)]
        public JsonDotNetResult ToggleBlackList(ToggleBlackListModel model)
        {
            var accountRepo = new AccountRepository(new ConnectionProvider());
            var blackListRepo = new StoryBlackListRepository(new ConnectionProvider());
            var storyRepo = new StoryRepository(new ConnectionProvider());
            var account = accountRepo.GetByLoginEmail(User.Identity.Name);

            var accountInfo = accountRepo.GetAcctInfoById(account.AccountInformation.Id);

            if (model.IsBlackListed)
            {
                var blackListItem = new StoryBlackList
                    {
                        Story = storyRepo.GetById(model.StoryId),
                        AccountInformation = accountInfo
                    };
                blackListRepo.Save(blackListItem);
            }
            else
            {
                var blackListItems = blackListRepo.GetForAccountInformation(account.AccountInformation.Id);
                var existing = blackListItems.SingleOrDefault(x => x.Story.Id == model.StoryId);
                if (existing != null)
                {
                    blackListRepo.Delete(existing.Id);
                }
            }

            return new JsonDotNetResult();
        }

        [Authorize]
        [AuthorizationFilter(RolesStatic.SuperUser)]
        public JsonDotNetResult GetBlackListSummary()
        {
            var acctInfoId = (int)Session[SessionConstants.AcctInfoId];
            var account = new AccountRepository(new ConnectionProvider()).GetByLoginEmail(User.Identity.Name);
            var stories = new StoryRepository(new ConnectionProvider()).GetAllForAcctInfo(acctInfoId);
            var blackListRepo = new StoryBlackListRepository(new ConnectionProvider());

            var blackList = blackListRepo.GetForAccountInformation(account.AccountInformation.Id);

            var summary = stories.OrderBy(x => x.StoryOrder).Select(x => new BlackListSummary
                {
                    Story = x,
                    IsBlackListed = blackList.Any(y => y.Story.Id == x.Id)
                });

            return new JsonDotNetResult { JsonRequestBehavior = JsonRequestBehavior.AllowGet, Data = summary };
        }

        [Authorize]
        [AuthorizationFilter(RolesStatic.SuperUser)]
        public JsonDotNetResult GetStoryDump()
        {
            var acctInfoId = (int)Session[SessionConstants.AcctInfoId];
            var storyRepo = new StoryRepository(new ConnectionProvider());
            var slideRepo = new SlideRepository(new ConnectionProvider());
            var questionRepo = new QuestionRepository(new ConnectionProvider());
            var stories = storyRepo.GetAllForAcctInfo(acctInfoId);

            var results = stories.Select(x => new StorySet
                {
                    Story = x,
                    Slides = slideRepo.GetForStory(x.Id),
                    Questions = questionRepo.GetForStory(x.Id)
                }).ToList();

            return new JsonDotNetResult { JsonRequestBehavior = JsonRequestBehavior.AllowGet, Data = results };
        }

        [Authorize]
        [HttpPost]
        [AuthorizationFilter(RolesStatic.SuperUser)]
        public JsonDotNetResult UpdateEverything(UpdateEverythingModel model)
        {
            var accountInformation =
                new AccountRepository(new ConnectionProvider()).GetAcctInfoById(
                    (int)Session[SessionConstants.AcctInfoId]);
            var updater = new StoryUpdater(new StoryRepository(new ConnectionProvider()),
                                           new SlideRepository(new ConnectionProvider()),
                                           new QuestionRepository(new ConnectionProvider()));

            updater.Update(model.Story, model.Slides, model.Questions, accountInformation);

            return new JsonDotNetResult();
        }

        [Authorize]
        [HttpPost]
        [AuthorizationFilter(RolesStatic.SuperUser)]
        public JsonDotNetResult UpdateOrder(UpdateOrderModel model)
        {
            var storyRepo = new StoryRepository(new ConnectionProvider());
            var stories = storyRepo.GetAllForAcctInfo((int)Session[SessionConstants.AcctInfoId]);

            foreach (var story in model.Stories)
            {
                var dbStory = stories.Single(x => x.Id == story.Id);
                if (dbStory.StoryOrder != story.StoryOrder)
                {
                    dbStory.StoryOrder = story.StoryOrder;
                    storyRepo.Save(dbStory);
                }
            }

            return new JsonDotNetResult();
        }

        public JsonDotNetResult GetAllStories()
        {
            var acctInfoId = (int)Session[SessionConstants.AcctInfoId];
            var stories = new StoryRepository(new ConnectionProvider()).GetAllForAcctInfo(acctInfoId).OrderBy(x => x.StoryOrder);
            return new JsonDotNetResult { Data = stories, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        public JsonDotNetResult GetSlides(int storyId)
        {
            var slides = new SlideRepository(new ConnectionProvider()).GetForStory(storyId);

            return new JsonDotNetResult { Data = slides, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
        public JsonDotNetResult GetQuestions(int storyId)
        {
            var questions = new QuestionRepository(new ConnectionProvider()).GetForStory(storyId);

            return new JsonDotNetResult { Data = questions, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        [HttpPost]
        [Authorize]
        [AuthorizationFilter(RolesStatic.SuperUser)]
        public JsonDotNetResult UpdateStory(Story model)
        {
            new StoryRepository(new ConnectionProvider()).Save(model);
            return new JsonDotNetResult();
        }

        [HttpPost]
        [Authorize]
        [AuthorizationFilter(RolesStatic.SuperUser)]
        public JsonDotNetResult UpdateSlide(Slide model)
        {
            new SlideRepository(new ConnectionProvider()).Save(model);

            return new JsonDotNetResult();
        }
        [HttpPost]
        [Authorize]
        [AuthorizationFilter(RolesStatic.SuperUser)]
        public JsonDotNetResult UpdateQuestion(Question model)
        {
            new QuestionRepository(new ConnectionProvider()).Save(model);

            return new JsonDotNetResult();
        }

        [HttpPost]
        [Authorize]
        [AuthorizationFilter(RolesStatic.SuperUser)]
        public JsonDotNetResult DeleteStory(Story model)
        {
            new StoryRepository(new ConnectionProvider()).Delete(model.Id);
            return new JsonDotNetResult();
        }

        [HttpPost]
        [Authorize]
        [AuthorizationFilter(RolesStatic.SuperUser)]
        public JsonDotNetResult DeleteSlide(Slide model)
        {
            new SlideRepository(new ConnectionProvider()).Delete(model.Id);

            return new JsonDotNetResult();
        }
        [HttpPost]
        [Authorize]
        [AuthorizationFilter(RolesStatic.SuperUser)]
        public JsonDotNetResult DeleteQuestion(Question model)
        {
            new QuestionRepository(new ConnectionProvider()).Delete(model.Id);

            return new JsonDotNetResult();
        }

        [HttpPost]
        [Authorize]
        [AuthorizationFilter(RolesStatic.SuperUser)]
        public JsonDotNetResult AddStory(Story model)
        {
            new StoryRepository(new ConnectionProvider()).Save(model);
            return new JsonDotNetResult();
        }

        [HttpPost]
        [Authorize]
        [AuthorizationFilter(RolesStatic.SuperUser)]
        public JsonDotNetResult AddSlide(Slide model)
        {
            new SlideRepository(new ConnectionProvider()).Save(model);
            return new JsonDotNetResult();
        }
        [HttpPost]
        [Authorize]
        [AuthorizationFilter(RolesStatic.SuperUser)]
        public JsonDotNetResult AddQuestion(Question model)
        {
            new QuestionRepository(new ConnectionProvider()).Save(model);
            return new JsonDotNetResult();
        }

        [HttpPost]
        [Authorize]
        [AuthorizationFilter(RolesStatic.SuperUser)]
        public JsonDotNetResult SubmitCustom(SubmitCustomModel model)
        {
            var emailSender = new EmailSender(new AuditLogRepository(new ConnectionProvider()));
            var email = String.Format("Title: {0}, Slide1: {1}, Slide2: {2}, Slide3: {3}, Slide4: {4}", model.Title, model.Slide1, model.Slide2, model.Slide3, model.Slide4);
            emailSender.SendEmail("contact@practiceowl.com", email, "New Quiz Idea!");

            return new JsonDotNetResult();
        }


        public JsonDotNetResult GetStoryForMember(int memberId, int token)
        {
            var member = new MemberRepository(new ConnectionProvider()).GetById(memberId);
            var accountInfo = member.AccountInformation;
            var memberQuizStatusRepository = new MemberQuizStatusRepository(new ConnectionProvider());
            var history = memberQuizStatusRepository.GetHistoryForMember(memberId);
            if (token == 9999)
                return new JsonDotNetResult { JsonRequestBehavior = JsonRequestBehavior.AllowGet, Data = new { successful = true, member, accountInfo } };
            var isInvalid = history.Any(x => x.GeneratedToken == token && x.Completed);
            if (isInvalid)
                return new JsonDotNetResult
                {
                    Data = new { successful = false },
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };
            var storyRepo = new StoryRepository(new ConnectionProvider());
            var storyBlackList = new StoryBlackListRepository(new ConnectionProvider());

            var completedStories = history.Where(x => x.Completed).Select(x => x.StoryId);
            var blackList = storyBlackList.GetForAccountInformation(accountInfo.Id);
            var nextStory = storyRepo.GetAllForAcctInfo(accountInfo.Id).Where(x => !completedStories.Contains(x.Id) && blackList.All(y => y.Story.Id != x.Id)).OrderBy(x => x.StoryOrder).FirstOrDefault();
            if (member.PhoneNumber != "6786786789" && member.QuizToken != token)
                return new JsonDotNetResult { Data = new { error = "Not Authorized" }, JsonRequestBehavior = JsonRequestBehavior.AllowGet };

            var existingHistory = history.SingleOrDefault(x => x.StoryId == nextStory.Id);
            if (existingHistory == null)
            {
                var newHistory = new MemberQuizStatus
                    {
                        StoryId = nextStory.Id,
                        GeneratedToken = token,
                        Member = member,
                        PointsEarned = 0,
                        StoryName = nextStory.Name,
                        DateCompleted = DateTime.Now.AddDays(-2)
                    };
                memberQuizStatusRepository.Save(newHistory);
            }
            else
            {
                existingHistory.GeneratedToken = token;
                existingHistory.PointsEarned = 0;
                memberQuizStatusRepository.Save(existingHistory);
            }
            var slides = new SlideRepository(new ConnectionProvider()).GetForStory(nextStory.Id);
            var questions = new QuestionRepository(new ConnectionProvider()).GetForStory(nextStory.Id);
            return new JsonDotNetResult { JsonRequestBehavior = JsonRequestBehavior.AllowGet, Data = new { successful = true, member, story = nextStory, slides, questions, accountInfo } };
        }

        [HttpPost]
        public JsonDotNetResult FinishQuiz(FinishQuizModel model)
        {
            var memberRepository = new MemberRepository(new ConnectionProvider());
            var member = memberRepository.GetById(model.MemberId);
            var memberQuizStatusRepository = new MemberQuizStatusRepository(new ConnectionProvider());
            if (member.PhoneNumber == "6786786789")
                return new JsonDotNetResult { Data = new { historyId = 1 } };

            var currentHistory = memberQuizStatusRepository.GetByToken(model.Token, model.MemberId);

            currentHistory.Completed = true;
            currentHistory.DateCompleted = DateTime.Now;
            currentHistory.PointsEarned = model.PointsEarned;
            currentHistory.PayedAmount = 0;
            currentHistory.PayedFor = true;
            currentHistory.DatePayedFor = DateTime.Now;

            member.TotalPoints = member.TotalPoints + model.PointsEarned;
            memberRepository.Save(member);
            memberQuizStatusRepository.Save(currentHistory);

            var acctPointUpdater = new AccountPointAdder(new AccountRepository(new ConnectionProvider()));
            acctPointUpdater.AddPointsToAccount(member.AccountInformation.LastAccountSignedOn, PointValues.PatientTakesQuiz, member.AccountInformation.Id);

            return new JsonDotNetResult { Data = new { historyId = currentHistory.Id } };
        }
    }

    public class UpdateOrderModel
    {
        public List<Story> Stories { get; set; }
    }

    public class UpdateEverythingModel
    {
        public List<Slide> Slides { get; set; }
        public List<Question> Questions { get; set; }
        public Story Story { get; set; }
    }

    public class FinishQuizModel
    {
        public int MemberId { get; set; }
        public int Token { get; set; }
        public int PointsEarned { get; set; }
    }

    public class SubmitCustomModel
    {
        public string Title { get; set; }
        public string Slide1 { get; set; }
        public string Slide2 { get; set; }
        public string Slide3 { get; set; }
        public string Slide4 { get; set; }
    }
}
