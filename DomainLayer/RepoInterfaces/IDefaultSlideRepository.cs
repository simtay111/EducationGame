using System.Collections.Generic;
using DomainLayer.Entities.Stories;

namespace DomainLayer.RepoInterfaces
{
    public interface IDefaultSlideRepository
    {
        List<DefaultSlide> GetForStory(int storyId);
        void Save(DefaultSlide model);
        void Delete(int id);
        List<DefaultSlide> GetAll();
    }
}