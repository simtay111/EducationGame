using System.Collections.Generic;
using DomainLayer.Entities.Stories;

namespace DomainLayer.RepoInterfaces
{
    public interface IStoryRepository
    {
        List<Story> GetAllForAcctInfo(int accountInfoId);
        void Save(Story model);
        void Delete(int id);
        Story GetById(int storyId);
        List<Story> GetPublicStories();
    }
}