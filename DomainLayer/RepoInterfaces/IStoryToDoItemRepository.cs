using System.Collections.Generic;
using DomainLayer.Entities;

namespace DomainLayer.RepoInterfaces
{
    public interface IStoryToDoItemRepository
    {
        void Save(StoryToDoItem storyToDoItem);
        List<StoryToDoItem> GetByMemberId(int memberId);
        void Delete(int storyToDoItemId);
        StoryToDoItem GetById(int storyToDoItemId);
    }
}