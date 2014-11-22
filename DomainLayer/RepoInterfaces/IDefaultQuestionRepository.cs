using System.Collections.Generic;
using DomainLayer.Entities.Stories;

namespace DomainLayer.RepoInterfaces
{
    public interface IDefaultQuestionRepository
    {
        List<DefaultQuestion> GetForStory(int storyId);
        void Save(DefaultQuestion model);
        void Delete(int id);
        List<DefaultQuestion> GetAll();
    }
}