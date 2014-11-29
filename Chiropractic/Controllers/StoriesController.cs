using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using DataLayer;
using DomainLayer.Authentication;
using DomainLayer.Email;
using DomainLayer.Entities;
using DomainLayer.Entities.Quizes;
using DomainLayer.Entities.Stories;
using DomainLayer.Points;
using DomainLayer.StoryManagement;
using EducationGame.Controllers.CustomResults;
using EducationGame.Filters;

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
