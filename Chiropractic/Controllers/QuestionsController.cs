using System.Web.Http;
using System.Web.Mvc;
using DataLayer;
using DomainLayer;
using DomainLayer.Entities.Stories;
using EducationGame.Controllers.CustomResults;

namespace EducationGame.Controllers
{
    [RoutePrefix("api/questions")]
    public class QuestionsController : ApiController
    {
        readonly QuestionRepository _questionRepo = new QuestionRepository(new ConnectionProvider());
        [Route("{id}")]
        public Question GetById(int id)
        {
            var questionRepo = new QuestionRepository(new ConnectionProvider());
            return questionRepo.GetById(id);
        }

        [System.Web.Http.HttpPost]
        [Route("{storyId}")]
        public void SaveQuestion(Question model, int storyId)
        {
            if (model.Id != SystemConstants.NewRecordId)
            {
                var question = _questionRepo.GetById(model.Id);
                question.AnswerBool = model.AnswerBool;
                question.CorrectAnswer = model.CorrectAnswer;
                question.WrongAnswer = model.WrongAnswer;
                question.Query = model.Query;
                _questionRepo.Save(question);
            }
            else
            {
                var storyRepo = new StoryRepository(new ConnectionProvider());
                model.Id = 0;
                model.Story = storyRepo.GetById(storyId);
                _questionRepo.Save(model);
            }
        }
        [System.Web.Http.HttpDelete]
        [Route("{id}")]
        public void Delete(int id)
        {
            _questionRepo.Delete(id);
        }
    }
}