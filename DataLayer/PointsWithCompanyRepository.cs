using System.Collections.Generic;
using System.Linq;
using DomainLayer.Entities.Quizes;
using DomainLayer.Entities.Stories;
using DomainLayer.RepoInterfaces;
using NHibernate;
using NHibernate.Linq;

namespace DataLayer
{
    public class PointsWithCompanyRepository : IPointsWithCompanyRepository
    {
        private readonly IConnectionProvider _connectionProvider;
        private readonly ISession _connection;

        public PointsWithCompanyRepository(IConnectionProvider connectionProvider)
        {
            _connectionProvider = connectionProvider;
            _connection = _connectionProvider.CreateConnection();
        }

        public void Save(PointsWithCompany model)
        {
            var connection = _connectionProvider.CreateConnection();
            connection.SaveOrUpdate(model);
        }

        public void Delete(int id)
        {
            var connection = _connectionProvider.CreateConnection();
            connection.Delete(connection.Load(typeof(PointsWithCompany), id));
        }

        public PointsWithCompany GetById(int storyId)
        {
            var connection = _connectionProvider.CreateConnection();
            return connection.Get<PointsWithCompany>(storyId);
        }

        public PointsWithCompany GetForMemberForAcct(int memberId, int accountInfoId)
        {
            return
                (from story in _connection.Query<PointsWithCompany>()
                 where story.AccountInformation.Id == accountInfoId &&
                       story.Member.Id == memberId
                 select story)
                    .FirstOrDefault();
        }
    }
}