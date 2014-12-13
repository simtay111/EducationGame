using System.Collections.Generic;
using DomainLayer.Entities;
using DomainLayer.RepoInterfaces;

namespace DomainLayer.Rewards
{
    public class GetPublicAvailableRewardsHandler
    {
        private readonly IPrizeRepository _prizeRepository;

        public GetPublicAvailableRewardsHandler(IPrizeRepository prizeRepository)
        {
            _prizeRepository = prizeRepository;
        }

        public GetPublicAvailableRewardsResponse Handle(GetPublicAvailableRewardsRequest request)
        {
            var prizes = _prizeRepository.GetAllPublicPrizes();

            return new GetPublicAvailableRewardsResponse {Prizes = prizes};
        }

    }

    public class GetPublicAvailableRewardsRequest
    {
    }

    public class GetPublicAvailableRewardsResponse
    {
        public List<AvailablePrize> Prizes { get; set; }
    }
}