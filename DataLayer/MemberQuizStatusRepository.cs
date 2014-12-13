using System;
using System.Collections.Generic;
using System.Linq;
using DomainLayer.Entities;
using DomainLayer.Entities.Quizes;
using DomainLayer.RepoInterfaces;
using NHibernate.Linq;

namespace DataLayer
{
    public class MemberQuizStatusRepository : IMemberQuizStatusRepository
    {
        private readonly IConnectionProvider _connectionProvider;

        public MemberQuizStatusRepository(IConnectionProvider connectionProvider)
        {
            _connectionProvider = connectionProvider;
        }

        public List<MemberQuizStatus> GetHistoryForMember(int memberId)
        {
            var connection = _connectionProvider.CreateConnection();
                return (from stats in connection.Query<MemberQuizStatus>() where stats.Member.Id == memberId select stats ).ToList() ;
        }

        public void Save(MemberQuizStatus status)
        {
            var connection = _connectionProvider.CreateConnection();
                connection.SaveOrUpdate(status);
                ;
        }

        public void Delete(int id)
        {
            var connection = _connectionProvider.CreateConnection();
                connection.Delete(connection.Load(typeof (MemberQuizStatus), id));
                ;
        }

        public MemberQuizStatus GetById(int statusId)
        {
            var connection = _connectionProvider.CreateConnection();
                return connection.Get<MemberQuizStatus>(statusId);
        }

        public MemberQuizStatus GetByStoryIdAndMemberId(int storyId, int memberId)
        {
            var connection = _connectionProvider.CreateConnection();
                return (from stats in connection.Query<MemberQuizStatus>() where stats.Member.Id == memberId && stats.StoryId == storyId select stats ).SingleOrDefault();
        }
    }
}