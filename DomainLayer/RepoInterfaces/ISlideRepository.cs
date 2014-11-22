using System.Collections.Generic;
using DomainLayer.Entities.Stories;

namespace DomainLayer.RepoInterfaces
{
    public interface ISlideRepository
    {
        List<Slide> GetForStory(int storyId);
        void Save(Slide model);
        void Delete(int id);
        List<Slide> GetAll();
    }
}