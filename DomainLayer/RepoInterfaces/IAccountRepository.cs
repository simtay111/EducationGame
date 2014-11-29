using System.Collections;
using System.Collections.Generic;
using DomainLayer.Entities;

namespace DomainLayer.RepoInterfaces
{
    public interface IAccountRepository : IDataAccess
    {
        void Delete(Account acct);
        void SaveAccountInformation(AccountInformation accountInformation);
        AccountInformation GetAccountInformation(string name);
        List<Account> GetAssistantAccountsFromPrimary(string primaryUserName);
        AccountInformation GetAcctInfoById(int accountInformationId);
        List<AccountInformation> GetAcctInfoThatIsTwoDaysOld();
        List<AccountInformation> GetAllAccountInformations();
        Account GetById(int id);
        void ActivateAllAccounts();
        List<AccountInformation> GetAcctInfoThatIsFiveDaysOld();
        List<Account> GetAssistantAccountsForAccountInformation(int id);
    }
}