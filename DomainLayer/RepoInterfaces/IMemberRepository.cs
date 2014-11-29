using System.Collections.Generic;
using DomainLayer.Entities;

namespace DomainLayer.RepoInterfaces
{
    public interface IMemberRepository
    {
        int Save(Member member);
        void Delete(int id);
        Member GetById(int memberId);
        int GetNumberOfQuizesTakenByMembers(List<int> memberIds);
        int GetTotalNumberOfPointsForMembers(List<int> memberIds);
        Member GetByLoginEmail(string userName);
    }
}