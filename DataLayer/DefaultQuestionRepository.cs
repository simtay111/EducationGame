using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DomainLayer.Entities.Stories;
using DomainLayer.RepoInterfaces;
using NHibernate.Linq;

namespace DataLayer
{
    public class DefaultQuestionRepository : IDefaultQuestionRepository
    {
        private readonly IConnectionProvider _connectionProvider;

        public DefaultQuestionRepository(IConnectionProvider connectionProvider)
        {
            _connectionProvider = connectionProvider;
        }

        public List<DefaultQuestion> GetForStory(int storyId)
        {
            var connection = _connectionProvider.CreateConnection();
                return
                    (from question in connection.Query<DefaultQuestion>() where question.Story.Id == storyId select question)
                        .ToList();
        }

        public void Save(DefaultQuestion model)
        {
            var connection = _connectionProvider.CreateConnection();
                connection.SaveOrUpdate(model);
                ;
        }

        public void Delete(int id)
        {
            var connection = _connectionProvider.CreateConnection();
                connection.Delete(connection.Load(typeof (DefaultQuestion), id));
                ;
        }

        public List<DefaultQuestion> GetAll()
        {
            var connection = _connectionProvider.CreateConnection();
                return
                    (from question in connection.Query<DefaultQuestion>() select question)
                        .ToList();
        }
    }
}