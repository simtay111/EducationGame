using System.Collections.Generic;
using DomainLayer.Entities.Stories;

namespace DomainLayer.RepoInterfaces
{
    public interface IQuestionRepository
    {
        List<Question> GetForStory(int storyId);
        void Save(Question model);
        void Delete(int id);
        List<Question> GetAll();
    }
}