using System.Collections.Generic;
using System.Linq;
using DomainLayer.Constants;
using DomainLayer.Entities;
using DomainLayer.RepoInterfaces;

namespace DomainLayer.Stories.State
{
    public class CurrentStoryStateProvider
    {
        private readonly IStoryToDoItemRepository _storyToDoItemRepository;

        public CurrentStoryStateProvider(IStoryToDoItemRepository storyToDoItemRepository)
        {
            _storyToDoItemRepository = storyToDoItemRepository;
        }

        public StoryToDoItem GetNextStep(int memberId, int storyId)
        {
            var itemsToDo = _storyToDoItemRepository.GetByMemberId(memberId);

            if (itemsToDo.Any(x => x.Type == ToDoType.Slide))
                return GetByFirstByType(itemsToDo, ToDoType.Slide);
            if (itemsToDo.Any(x => x.Type == ToDoType.Question))
                return GetByFirstByType(itemsToDo, ToDoType.Question);

            return null;
        }

        private StoryToDoItem GetByFirstByType(List<StoryToDoItem> items, ToDoType typeToGet)
        {
            return items.Where(x => x.Type == typeToGet).OrderBy(x => x.Id).First();
        }
    }
}