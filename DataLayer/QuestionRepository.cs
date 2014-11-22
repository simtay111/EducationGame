using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DomainLayer.Entities.Stories;
using DomainLayer.RepoInterfaces;
using NHibernate.Linq;

namespace DataLayer
{
    public class QuestionRepository : IQuestionRepository
    {
        private readonly IConnectionProvider _connectionProvider;

        public QuestionRepository(IConnectionProvider connectionProvider)
        {
            _connectionProvider = connectionProvider;
        }

        public List<Question> GetForStory(int storyId)
        {
            var connection = _connectionProvider.CreateConnection();
                return
                    (from question in connection.Query<Question>() where question.Story.Id == storyId select question)
                        .ToList();
        }

        public void Save(Question model)
        {
            var connection = _connectionProvider.CreateConnection();
                connection.SaveOrUpdate(model);
                ;
        }

        public void Delete(int id)
        {
            var connection = _connectionProvider.CreateConnection();
                connection.Delete(connection.Load(typeof (Question), id));
                ;
        }

        public List<Question> GetAll()
        {
            var connection = _connectionProvider.CreateConnection();
                return
                    (from question in connection.Query<Question>() select question)
                        .ToList();
        }
    }
}