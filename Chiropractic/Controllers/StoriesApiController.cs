using System.Net.Http;
using System.Web;
using System.Web.Http;
using DataLayer;
using DomainLayer;
using DomainLayer.Entities.Stories;

namespace EducationGame.Controllers
{
    [RoutePrefix("api/stories")]
    public class StoriesApiController : ApiController
    {
        readonly StoryRepository _storyRepository = new StoryRepository(new ConnectionProvider());

        [Route("{id}")]
        public Story GetById(int id)
        {
            return _storyRepository.GetById(id);
        }

        [Route("{id}")]
        [HttpPost]
        public void Save(Story model, int id, HttpRequestMessage request)
        {
            if (id != SystemConstants.NewRecordId)
            {
                var story = _storyRepository.GetById(id);
                story.IsPublic = model.IsPublic;
                story.Name = model.Name;
                story.Summary = model.Summary;
                story.MessageLessonText = model.MessageLessonText;
                _storyRepository.Save(story);
            }
            else
            {
                var accountsRepo = new AccountRepository(new ConnectionProvider());
                model.Id = 0;
                model.AccountInformation = accountsRepo.GetAccountInformation((int)Session

            }
        }
    }
}