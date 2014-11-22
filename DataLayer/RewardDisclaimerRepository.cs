using System.Collections.Generic;
using System.Linq;
using DomainLayer.Entities;
using NHibernate.Linq;

namespace DataLayer
{
    public class RewardDisclaimerRepository 
    {
        private readonly IConnectionProvider _connectionProvider;

        public RewardDisclaimerRepository(IConnectionProvider connectionProvider)
        {
            _connectionProvider = connectionProvider;
        }

        public List<RewardDisclaimer> GetAll()
        {
            var connection = _connectionProvider.CreateConnection();
            return
                (from story in connection.Query<RewardDisclaimer>() select story)
                    .ToList();
        }
    }
}