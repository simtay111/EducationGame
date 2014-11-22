using System;
using System.Collections.Generic;
using DomainLayer.Entities.Quizes;

namespace DomainLayer.RepoInterfaces
{
    public interface IMemberQuizStatusRepository
    {
        List<MemberQuizStatus> GetHistoryForMember(int memberId);
        void Save(MemberQuizStatus status);
        void Delete(int id);
        MemberQuizStatus GetById(int statusId);
        MemberQuizStatus GetByToken(int token, int memberId);
        List<MemberQuizStatus> GetLatestFiveCompletedQuizHistories(int accountInformationId, DateTime fromTime);
        List<MemberQuizStatus> GetInRange(int acctInfoId, DateTime start, DateTime end);
        List<MemberQuizStatus> GetNonPayedForCompletedQuizzes();
    }
}