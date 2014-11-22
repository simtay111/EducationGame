using System.Collections.Generic;
using DomainLayer.Entities;

namespace DomainLayer.RepoInterfaces
{
    public interface IPrizeRepository
    {
        List<AvailablePrize> GetAllPublicPrizes();
        void Save(CustomPrize model);
        void Delete(int id);
        void UpdateGroup(List<CustomPrize> prizes);
        List<CustomPrize> GetForAccount(int accountInfoId);
    }
}