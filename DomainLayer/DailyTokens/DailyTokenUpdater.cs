using System;
using System.Collections.Generic;
using DomainLayer.Entities;
using DomainLayer.RepoInterfaces;

namespace DomainLayer.DailyTokens
{
    public class DailyTokenUpdater
    {
        private readonly IAccountRepository _accountRepository;

        public DailyTokenUpdater(IAccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
        }

        public void Update()
        {
            var accountInfos = _accountRepository.GetAllAccountInformations();
            var random = new Random();
            foreach (var acct in accountInfos)
            {
                if (acct.DateOfDailyToken.Date != DateTime.Now.Date)
                {
                    acct.DateOfDailyToken = DateTime.Now.Date;
                    acct.DailyToken = random.Next(1000, 9999);
                    acct.SentDailyPrintout = false;
                    _accountRepository.SaveAccountInformation(acct);
                    
                }
            }
        }
    }
}