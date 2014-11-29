using System;
using System.Collections.Generic;
using DomainLayer.Entities;

namespace DomainLayer.RepoInterfaces
{
    public interface IAwardedPrizeRepository
    {
        List<AwardedPrize> GetAll();
        AwardedPrize GetById(int id);
        void Save(AwardedPrize model);
        void Delete(int id);
        void UpdateGroup(List<AwardedPrize> prizes );
        List<AwardedPrize> GetForMember(int memberId);
        List<AwardedPrize> GetOrdered();
        List<AwardedPrize> GetNonBilledRedeemedTangoAwards();
    }
}