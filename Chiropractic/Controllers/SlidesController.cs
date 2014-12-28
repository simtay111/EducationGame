using System.Web.Http;
using System.Web.Mvc;
using DataLayer;
using DomainLayer;
using DomainLayer.Entities.Stories;
using EducationGame.Controllers.CustomResults;

namespace EducationGame.Controllers
{
    [RoutePrefix("api/slides")]
    public class SlidesController : ApiController
    {
        readonly SlideRepository _slideRepo = new SlideRepository(new ConnectionProvider());

        [Route("{id}")]
        public Slide GetSlide(int id)
        {
            return _slideRepo.GetById(id);
        }

        [System.Web.Mvc.HttpPost]
        [Route("{storyId}")]
        public void SaveSlide(Slide model, int storyId)
        {
            if (model.Id != SystemConstants.NewRecordId)
            {
                var slide = _slideRepo.GetById(model.Id);
                slide.Body = model.Body;
                slide.Title = model.Title;
                _slideRepo.Save(slide);
            }
            else
            {
                var storyRepo = new StoryRepository(new ConnectionProvider());
                model.Story = storyRepo.GetById(storyId);
                
                model.Id = 0;
                _slideRepo.Save(model);
            }
        }

        [System.Web.Http.HttpDelete]
        [Route("{id}")]
        public void Delete(int id)
        {
            _slideRepo.Delete(id);
        }
    }
}