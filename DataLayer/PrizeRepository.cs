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

        public void Save(CustomPrize model)
        {
            var connection = _connectionProvider.CreateConnection();
            connection.SaveOrUpdate(model);
        }

        public void Delete(int id)
        {
            var connection = _connectionProvider.CreateConnection();
            connection.Delete(connection.Load(typeof(CustomPrize), id));
        }
        public void Save(AvailablePrize model)
        {
            var connection = _connectionProvider.CreateConnection();
            connection.SaveOrUpdate(model);
        }

        public List<AvailablePrize> GetAllPublicPrizes()
        {
            var connection = _connectionProvider.CreateConnection();
            return (from prize in connection.Query<AvailablePrize>() select prize).ToList();
        }

        public void UpdateGroup(List<CustomPrize> prizes)
        {
            var connection = _connectionProvider.CreateConnection();
            foreach (var prize in prizes)
            {
                connection.SaveOrUpdate(prize);
            }
        }

        public List<CustomPrize> GetForAccount(int accountInfoId)
        {
            var connection = _connectionProvider.CreateConnection();
            return
                (from prize in connection.Query<CustomPrize>()
                 where prize.AccountInformation.Id == accountInfoId
                 select prize).ToList();
        }

        public void DeleteAvailable(int id)
        {
            var connection = _connectionProvider.CreateConnection();
            connection.Delete(connection.Load(typeof(AvailablePrize), id));
        }
    }
}