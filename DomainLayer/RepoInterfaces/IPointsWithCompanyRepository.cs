using DomainLayer.Entities.Quizes;

namespace DomainLayer.RepoInterfaces
{
    public interface IPointsWithCompanyRepository
    {
        PointsWithCompany GetForMemberForAcct(int memberId, int accountInfoId);
        void Save(PointsWithCompany pointsForMember);
    }
}