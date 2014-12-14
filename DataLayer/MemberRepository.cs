using System.Collections.Generic;
using System.Linq;
using DomainLayer.Entities;
using DomainLayer.Entities.Quizes;
using DomainLayer.RepoInterfaces;
using NHibernate;
using NHibernate.Linq;

namespace DataLayer
{
    public class MemberRepository : IMemberRepository, ILogginEntityProvider
    {
        private readonly IConnectionProvider _connectionProvider;
        private readonly ISession _connection;

        public MemberRepository(IConnectionProvider connectionProvider)
        {
            _connectionProvider = connectionProvider;
            _connection = _connectionProvider.CreateConnection();
        }

        public void Delete(int id)
        {
            var quizStatusHistory = (from history in _connection.Query<MemberQuizStatus>()
                                     where history.Member.Id == id
                                     select history).ToList();
            foreach (var history in quizStatusHistory)
            {
                _connection.Delete(history);
            }
            var awardedPrizes = (from awardedPrize in _connection.Query<AwardedPrize>()
                                 where awardedPrize.Member.Id == id
                                 select awardedPrize).ToList();
            foreach (var prize in awardedPrizes)
            {
                _connection.Delete(prize);
            }
            _connection.Delete(_connection.Load(typeof(Member), id));
        }

        public Member GetById(int memberId)
        {
            return _connection.Get<Member>(memberId);
        }

        public int GetNumberOfQuizesTakenByMembers(List<int> memberIds)
        {
            return
                (from status in _connection.Query<MemberQuizStatus>()
                 where memberIds.Contains(status.Member.Id) && status.Completed
                 select status).Count();
        }


        public Member GetByLoginEmail(string email)
        {
            return GetUserByEmail(email);
        }

        private Member GetUserByEmail(string email)
        {
            return (from acct in _connection.Query<Member>() where acct.Email == email.ToUpper() select acct).SingleOrDefault();
        }

        public void Save<T>(T entityToSave)
        {
            _connection.SaveOrUpdate(entityToSave);
        }

        IHaveAuthorizationCredentials ILogginEntityProvider.GetByLoginEmail(string email)
        {
            return GetByLoginEmail(email);
        }
    }
}