using System.Collections.Generic;
using System.Linq;
using DomainLayer.Entities.Stories;
using DomainLayer.RepoInterfaces;
using NHibernate.Linq;

namespace DataLayer
{
    public class SlideRepository : ISlideRepository
    {
        private readonly IConnectionProvider _connectionProvider;

        public SlideRepository(IConnectionProvider connectionProvider)
        {
            _connectionProvider = connectionProvider;
        }

        public List<Slide> GetForStory(int storyId)
        {
            var connection = _connectionProvider.CreateConnection();
            return
               (from question in connection.Query<Slide>() where question.Story.Id == storyId select question)
                   .ToList();
        }

        public void Save(Slide model)
        {
            var connection = _connectionProvider.CreateConnection();
            connection.SaveOrUpdate(model);
        }

        public void Delete(int id)
        {
            var connection = _connectionProvider.CreateConnection();
            connection.Delete(connection.Load(typeof(Slide), id));
        }
        public List<Slide> GetAll()
        {
            var connection = _connectionProvider.CreateConnection();
            return
                (from slide in connection.Query<Slide>() select slide)
                    .ToList();
        }
    }
}