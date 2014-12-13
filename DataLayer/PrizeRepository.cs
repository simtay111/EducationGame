using System.Collections.Generic;
using System.Linq;
using DomainLayer.Entities;
using DomainLayer.RepoInterfaces;
using NHibernate.Linq;

namespace DataLayer
{
    public class PrizeRepository : IPrizeRepository
    {
        private readonly IConnectionProvider _connectionProvider;

        public PrizeRepository(IConnectionProvider connectionProvider)
        {
            _connectionProvider = connectionProvider;
        }

        public void Save(AvailablePrize model)
        {
            var connection = _connectionProvider.CreateConnection();
            connection.SaveOrUpdate(model);
        }

        public void Delete(int id)
        {
            var connection = _connectionProvider.CreateConnection();
            connection.Delete(connection.Load(typeof(AvailablePrize), id));
        }

        public List<AvailablePrize> GetAllPublicPrizes()
        {
            var connection = _connectionProvider.CreateConnection();
            return (from prize in connection.Query<AvailablePrize>() where prize.IsPublic select prize).ToList();
        }
    }
}