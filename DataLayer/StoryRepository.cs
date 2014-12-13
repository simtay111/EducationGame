using System.Collections.Generic;
using System.Linq;
using DomainLayer.Entities.Stories;
using DomainLayer.RepoInterfaces;
using NHibernate.Linq;

namespace DataLayer
{
    public class StoryRepository : IStoryRepository
    {
        private readonly IConnectionProvider _connectionProvider;

        public StoryRepository(IConnectionProvider connectionProvider)
        {
            _connectionProvider = connectionProvider;
        }

        public List<Story> GetAllForAcctInfo(int accountInfoId)
        {
            var connection = _connectionProvider.CreateConnection();
            return
                (from story in connection.Query<Story>() where story.AccountInformation.Id == accountInfoId select story)
                    .ToList();
        }

        public void Save(Story model)
        {
            var connection = _connectionProvider.CreateConnection();
            connection.SaveOrUpdate(model);
            ;
        }

        public void Delete(int id)
        {
            var connection = _connectionProvider.CreateConnection();
            connection.Delete(connection.Load(typeof(Story), id));
        }

        public Story GetById(int storyId)
        {
            var connection = _connectionProvider.CreateConnection();
            return connection.Get<Story>(storyId);
        }

        public List<Story> GetPublicStories()
        {
            var connection = _connectionProvider.CreateConnection();
            return
                (from story in connection.Query<Story>() where story.IsPublic select story)
                    .ToList();
        }
    }
}