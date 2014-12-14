using System.Collections.Generic;
using DomainLayer.Entities;

namespace DomainLayer.RepoInterfaces
{
    public interface IMemberRepository
    {
        void Delete(int id);
        Member GetById(int memberId);
        int GetNumberOfQuizesTakenByMembers(List<int> memberIds);
    }
}