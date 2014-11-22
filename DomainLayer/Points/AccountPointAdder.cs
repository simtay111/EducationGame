using System;
using DomainLayer.Entities;
using DomainLayer.RepoInterfaces;

namespace DomainLayer.Points
{
    public class AccountPointAdder
    {
        private readonly IAccountRepository _accountRepository;

        public AccountPointAdder(IAccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
        }

        public Account AddPointsToAccount(int accountId, PointValues points, int accountInfoId)
        {
            Account acct = null;
            if (points == PointValues.PrintDaily)
            {
                acct = _accountRepository.GetById(accountId);
                var acctInfo = _accountRepository.GetAcctInfoById(accountInfoId);
                if (acctInfo.DateOfPrintedDaily.Date != DateTime.Now.Date)
                {
                    acct.Points += (int)points;
                    acctInfo.DateOfPrintedDaily = DateTime.Now;
                    _accountRepository.SaveAccountInformation(acctInfo);
                }
            }
            else if (points == PointValues.PatientTakesQuiz)
            {
                acct = _accountRepository.GetById(accountId);
                if (acct != null)
                {
                    acct.Points += (int)points;
                }
            }
            else
            {
                acct = _accountRepository.GetById(accountId);
                acct.Points += (int)points;
            }


            _accountRepository.Save(acct);

            return acct;
        }
    }
}