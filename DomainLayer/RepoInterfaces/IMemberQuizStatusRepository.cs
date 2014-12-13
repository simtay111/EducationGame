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
        MemberQuizStatus GetByStoryIdAndMemberId(int storyId, int memberId);
    }
}