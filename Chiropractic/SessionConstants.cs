using DataLayer;
using DomainLayer.Entities;

namespace EducationGame
{
    public class SessionConstants
    {
        public const string AccountId = "ACCOUNTID";
        public const string AcctPermissionLevel = "ACCTPERMLEVEL";
        public const string IsAccount = "ISACCOUNT";

        public static int GetAccountInfoId(int accountId)
        {
            var account = new AccountRepository(new ConnectionProvider()).GetById(accountId);
            return account.AccountInformation.Id;
        }
    }
}