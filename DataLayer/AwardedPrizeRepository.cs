using System;
using System.Collections.Generic;
using System.Linq;
using DomainLayer.Entities;
using DomainLayer.RepoInterfaces;
using NHibernate.Linq;

namespace DataLayer
{
    public class AwardedPrizeRepository : IAwardedPrizeRepository
    {
        private readonly IConnectionProvider _connectionProvider;

        public AwardedPrizeRepository(IConnectionProvider connectionProvider)
        {
            _connectionProvider = connectionProvider;
        }

        public List<AwardedPrize> GetAll()
        {
            var connection = _connectionProvider.CreateConnection();
                return (from prize in connection.Query<AwardedPrize>() select prize).ToList();
        }
        public AwardedPrize GetById(int id)
        {
            var connection = _connectionProvider.CreateConnection();
                return connection.Get<AwardedPrize>(id);
        }

        public void Save(AwardedPrize model)
        {
            var connection = _connectionProvider.CreateConnection();
                connection.SaveOrUpdate(model);
                ;
        }

        public void Delete(int id)
        {
            var connection = _connectionProvider.CreateConnection();
                connection.Delete(connection.Load(typeof (AwardedPrize), id));
                ;
        }

        public void UpdateGroup(List<AwardedPrize> prizes )
        {
            var connection = _connectionProvider.CreateConnection();
                foreach (var prize in prizes)
                {
                    connection.SaveOrUpdate(prize);
                    ;
                }
        }

        public List<AwardedPrize> GetForMember(int memberId)
        {
            var connection = _connectionProvider.CreateConnection();
                return
                    (from prize in connection.Query<AwardedPrize>()
                     where prize.Member.Id == memberId
                     select prize).ToList();
        }
        public List<AwardedPrize> GetInRange(int acctInfoId, DateTime start, DateTime end)
        {
            var connection = _connectionProvider.CreateConnection();
                return (from awardedPrize in connection.Query<AwardedPrize>()
                        where awardedPrize.Member.AccountInformation.Id == acctInfoId
                       && awardedPrize.IssueDate>= start && awardedPrize.IssueDate <= end.Date.AddHours(24)
                        select awardedPrize).ToList();
        }

        public List<AwardedPrize> GetOrdered()
        {
            var connection = _connectionProvider.CreateConnection();
                return (from awardedPrize in connection.Query<AwardedPrize>()
                        where awardedPrize.Ordered
                        select awardedPrize).ToList();
        }

        public List<AwardedPrize> GetNonBilledRedeemedTangoAwards()
        {
            var connection = _connectionProvider.CreateConnection();
                return (from awardedPrize in connection.Query<AwardedPrize>()
                        where !awardedPrize.BilledToOffice && awardedPrize.PrizeSku != null && awardedPrize.Redeemed
                        select awardedPrize).ToList();
        }
    }
}