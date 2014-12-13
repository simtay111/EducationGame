using System.Collections.Generic;
using System.Linq;
using DomainLayer.Entities;
using DomainLayer.Entities.Stories;
using DomainLayer.RepoInterfaces;
using NHibernate.Linq;

namespace DataLayer
{
    public class StoryToDoItemRepository : IStoryToDoItemRepository
    {
        private readonly IConnectionProvider _connectionProvider;

        public StoryToDoItemRepository(IConnectionProvider connectionProvider)
        {
            _connectionProvider = connectionProvider;
        }

        public void Save(StoryToDoItem model)
        {
            var connection = _connectionProvider.CreateConnection();
            connection.SaveOrUpdate(model);
        }

        public List<StoryToDoItem> GetByMemberId(int memberId)
        {
            var connection = _connectionProvider.CreateConnection();
            return
                (from story in connection.Query<StoryToDoItem>() where story.Member.Id == memberId select story)
                    .ToList();
        }

        public void Delete(int id)
        {
            var connection = _connectionProvider.CreateConnection();
            connection.Delete(connection.Load(typeof(StoryToDoItem), id));
        }
    }
}