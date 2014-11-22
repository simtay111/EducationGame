using System.Collections.Generic;
using System.Linq;
using DomainLayer.Entities;
using NHibernate.Linq;

namespace DataLayer
{
    public class StoryBlackListRepository
    {
        private readonly IConnectionProvider _connectionProvider;

        public StoryBlackListRepository(IConnectionProvider connectionProvider)
        {
            _connectionProvider = connectionProvider;
        }

        public List<StoryBlackList> GetAll()
        {
            var connection = _connectionProvider.CreateConnection();
            return
                (from story in connection.Query<StoryBlackList>() select story)
                    .ToList();
        }

        public void Save(StoryBlackList model)
        {
            var connection = _connectionProvider.CreateConnection();
            connection.SaveOrUpdate(model);
        }

        public void Delete(int id)
        {
            var connection = _connectionProvider.CreateConnection();
            connection.Delete(connection.Load(typeof(StoryBlackList), id));
        }

        public List<StoryBlackList> GetForAccountInformation(int id)
        {
            var connection = _connectionProvider.CreateConnection();
            return
                (from story in connection.Query<StoryBlackList>() where story.AccountInformation.Id == id select story)
                    .ToList();
        }
    }
}