using System.Collections.Generic;
using DomainLayer.Entities.Stories;

namespace DomainLayer.RepoInterfaces
{
    public interface IDefaultStoryRepository
    {
        List<DefaultStory> GetAll();
        void Save(DefaultStory model);
        void Delete(int id);
        DefaultStory GetById(int storyId);
    }
}