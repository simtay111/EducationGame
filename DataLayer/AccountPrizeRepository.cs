using System.Collections.Generic;
using System.Linq;
using DomainLayer.Entities;
using NHibernate.Linq;

namespace DataLayer
{
    public class AccountPrizeRepository
    {
        private readonly IConnectionProvider _connectionProvider;

        public AccountPrizeRepository(IConnectionProvider connectionProvider)
        {
            _connectionProvider = connectionProvider;
        }

        public List<AccountPrize> GetAll()
        {
            var connection = _connectionProvider.CreateConnection();
            return (from prize in connection.Query<AccountPrize>() select prize).ToList();
        }
        public AccountPrize GetById(int id)
        {
            var connection = _connectionProvider.CreateConnection();
            return connection.Get<AccountPrize>(id);
        }

        public void Save(AccountPrize model)
        {
            var connection = _connectionProvider.CreateConnection();
            connection.SaveOrUpdate(model);
            ;
        }

        public void Delete(int id)
        {
            var connection = _connectionProvider.CreateConnection();
            connection.Delete(connection.Load(typeof(AccountPrize), id));
            ;
        }
    }
}