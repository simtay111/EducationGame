using System.Linq;
using DomainLayer.RepoInterfaces;

namespace DomainLayer.Reports
{
    public class AccountInfoDataUpdater
    {
        private readonly IAccountRepository _accountRepository;
        private readonly IMemberRepository _memberRepository;

        public AccountInfoDataUpdater(IAccountRepository accountRepository, IMemberRepository memberRepository)
        {
            _accountRepository = accountRepository;
            _memberRepository = memberRepository;
        }

        public void Update(int accountInfoId)
        {
            var acctInfo = _accountRepository.GetAcctInfoById(accountInfoId);
            var acctMembers = _memberRepository.GetMembersByAccountInfo(accountInfoId, includeInactive:true);

            acctInfo.NumberOfPatients = acctMembers.Count;
            acctInfo.NumberOfQuizesTaken =
                _memberRepository.GetNumberOfQuizesTakenByMembers(acctMembers.Select(x => x.Id).ToList());
            var totalPoints = _memberRepository.GetTotalNumberOfPointsForMembers(acctMembers.Select(x => x.Id).ToList());
            acctInfo.Gpa = (acctInfo.NumberOfQuizesTaken > 0) ? totalPoints/acctInfo.NumberOfQuizesTaken : 0;

            _accountRepository.SaveAccountInformation(acctInfo);
        }
    }
}