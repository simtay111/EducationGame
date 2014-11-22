using System.Collections.Generic;
using System.Linq;
using DomainLayer.Entities;
using DomainLayer.Entities.Quizes;
using DomainLayer.RepoInterfaces;
using NHibernate;
using NHibernate.Linq;

namespace DataLayer
{
    public class MemberRepository : IMemberRepository
    {
        private readonly IConnectionProvider _connectionProvider;
        private ISession _connection;

        public MemberRepository(IConnectionProvider connectionProvider)
        {
            _connectionProvider = connectionProvider;
            _connection = _connectionProvider.CreateConnection();
        }

        public List<Member> GetMembersByAccountInfo(int accountInfoId, bool includeInactive)
        {
            if (includeInactive)
            {
                return
                    (from members in _connection.Query<Member>()
                     where members.AccountInformation.Id == accountInfoId
                     select members).ToList();
            }
            return
                (from members in _connection.Query<Member>()
                 where members.AccountInformation.Id == accountInfoId && !members.Inactive
                 select members).ToList();
        }

        public int Save(Member member)
        {
            _connection.SaveOrUpdate(member);
            return member.Id;
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

        public int GetTotalNumberOfPointsForMembers(List<int> memberIds)
        {
            var connection = _connectionProvider.CreateConnection();
            var statusi = (from status in _connection.Query<MemberQuizStatus>()
                           where memberIds.Contains(status.Member.Id) && status.Completed
                           select status.PointsEarned).ToList();

            if (statusi.Any()) return statusi.Sum();
            return 0;
        }

        public void ChangeActievState(int memberId, bool isInactive)
        {
            var member = _connection.Get<Member>(memberId);
            member.Inactive = isInactive;
            _connection.Update(member);
        }

        public List<Member> GetByPhoneNumber(string phoneNumber)
        {
            return
                (from member in _connection.Query<Member>()
                 where member.PhoneNumber == phoneNumber.Trim()
                 select member).ToList();
        }

        public Member GetMemberByPhoneAndAccountId(string phoneNumber, int accountInfoId)
        {
            return
                    (from member in _connection.Query<Member>()
                     where member.PhoneNumber == phoneNumber.Trim() && member.AccountInformation.Id == accountInfoId
                     select member).FirstOrDefault();
        }
    }
}