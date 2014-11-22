using System.Collections.Generic;
using System.Linq;
using DomainLayer.Entities;
using DomainLayer.Entities.Stories;
using DomainLayer.RepoInterfaces;
using NHibernate.Linq;

namespace DataLayer
{
    public class DefaultStoryRepository : IDefaultStoryRepository
    {
        private readonly IConnectionProvider _connectionProvider;

        public DefaultStoryRepository(IConnectionProvider connectionProvider)
        {
            _connectionProvider = connectionProvider;
        }

        public List<DefaultStory> GetAll()
        {
            var connection = _connectionProvider.CreateConnection();
                return
                    (from story in connection.Query<DefaultStory>() select story)
                        .ToList();
        }

        public void Save(DefaultStory model)
        {
            var connection = _connectionProvider.CreateConnection();
                connection.SaveOrUpdate(model);
                ;
        }

        public void Delete(int id)
        {
            var connection = _connectionProvider.CreateConnection();
                connection.Delete(connection.Load(typeof(DefaultStory), id));
                ;
        }

        public DefaultStory GetById(int storyId)
        {
            var connection = _connectionProvider.CreateConnection();
                return connection.Get<DefaultStory>(storyId);
        }
    }
}