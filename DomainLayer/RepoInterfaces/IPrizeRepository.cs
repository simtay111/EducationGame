using System.Collections.Generic;
using DomainLayer.Entities;

namespace DomainLayer.RepoInterfaces
{
    public interface IPrizeRepository
    {
        List<AvailablePrize> GetAllPublicPrizes();
        void Delete(int id);
    }
}