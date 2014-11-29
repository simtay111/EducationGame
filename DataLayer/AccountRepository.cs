using System;
using System.Collections.Generic;
using System.Linq;
using DomainLayer.Authentication;
using DomainLayer.Entities;
using DomainLayer.RepoInterfaces;
using NHibernate;
using NHibernate.Linq;

namespace DataLayer
{
    public class AccountRepository : IAccountRepository, ILogginEntityProvider
    {
        private readonly IConnectionProvider _connectionProvider;
        private ISession _connection;

        public AccountRepository(IConnectionProvider connectionProvider)
        {
            _connectionProvider = connectionProvider;
            _connection = _connectionProvider.CreateConnection();
        }

        public void Delete(Account acct)
        {
            _connection.Delete(acct);
        }

        private Account GetUserByEmail(string email)
        {
            return (from acct in _connection.Query<Account>() where acct.Email == email.ToUpper() select acct).SingleOrDefault();
        }

        public void Save<T>(T account)
        {
            _connection.SaveOrUpdate(account);
        }

        public IHaveAuthorizationCredentials GetByLoginEmail(string email)
        {
            return (IHaveAuthorizationCredentials)GetUserByEmail(email);
        }

        public void SaveAccountInformation(AccountInformation accountInformation)
        {
            _connection.SaveOrUpdate(accountInformation);
        }

        public AccountInformation GetAccountInformation(string name)
        {
            var account = GetUserByEmail(name);

            return account.AccountInformation;
        }

        public List<Account> GetAssistantAccountsFromPrimary(string primaryUserName)
        {
            var priamry = GetUserByEmail(primaryUserName);
            return (from accounts in _connection.Query<Account>()
                    where
                        accounts.AccountInformation.Id == priamry.AccountInformation.Id &&
                        accounts.PermissionLevel < RolesStatic.SuperUser
                    select accounts).ToList();
        }

        public AccountInformation GetAcctInfoById(int accountInformationId)
        {
            return _connection.Get<AccountInformation>(accountInformationId);
        }

        public List<AccountInformation> GetAcctInfoThatIsTwoDaysOld()
        {
            return (from accounts in _connection.Query<AccountInformation>()
                    where
                        accounts.CreationDate < DateTime.Now.AddDays(-2)
                        && accounts.CreationDate > DateTime.Now.AddDays(-3)

                    select accounts).ToList();
        }
        public List<AccountInformation> GetAllAccountInformations()
        {
            return
                (from accountInformation in _connection.Query<AccountInformation>() select accountInformation)
                    .ToList();
        }

        public Account GetById(int id)
        {
            return _connection.Get<Account>(id);
        }

        public void ActivateAllAccounts()
        {
            _connection.CreateSQLQuery("update accountInformation set IsVerified = 1").ExecuteUpdate();
        }

        public List<AccountInformation> GetAcctInfoThatIsFiveDaysOld()
        {
            return (from accounts in _connection.Query<AccountInformation>()
                    where
                        accounts.CreationDate < DateTime.Now.AddDays(-5)
                        && accounts.CreationDate > DateTime.Now.AddDays(-6)

                    select accounts).ToList();
        }

        public List<Account> GetAssistantAccountsForAccountInformation(int id)
        {
            return (from accounts in _connection.Query<Account>()
                    where
                        accounts.AccountInformation.Id== id &&
                        accounts.PermissionLevel < RolesStatic.SuperUser
                    select accounts).ToList();
        }
    }
}