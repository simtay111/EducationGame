using System;
using System.Linq;
using DomainLayer.Entities;
using DomainLayer.OrderProcessing;
using DomainLayer.RepoInterfaces;

namespace DomainLayer.Accounts
{
    public interface IAccountCompletionStateGetter
    {
        AccountCompletionStatus GetStatus(int acctInfoId, string userEmail);
    }

    public class AccountCompletionStateGetter : IAccountCompletionStateGetter
    {
        private readonly IAccountRepository _accountRepository;
        private readonly SubscriptionCharger _subscriptionCharger;

        public AccountCompletionStateGetter(IAccountRepository accountRepository, SubscriptionCharger subscriptionCharger)
        {
            _accountRepository = accountRepository;
            _subscriptionCharger = subscriptionCharger;
        }

        public AccountCompletionStatus GetStatus(int acctInfoId, string userEmail)
        {
            var acctInfo = _accountRepository.GetAcctInfoById(acctInfoId);
            var status = new AccountCompletionStatus();
            status.BasicInfoIsComplete = (IsOfficeNameNotEmpty(acctInfo) && IsOfficePhoneNotEmpty(acctInfo));
            status.PaymentSetupIsComplete = IsTheAccountPayedFor(acctInfo);
            status.NotifyEmailsIsComplete = (IsNotifyEmail1NotEmpty(acctInfo));
            status.AssistantAccountsNeedsSome = (AreThereNoAssistantAccounts(userEmail));
            status.AccountIsUsable = status.BasicInfoIsComplete && status.PaymentSetupIsComplete;
            return status;
        }

        private bool AreThereNoAssistantAccounts(string userEmail)
        {
            if (string.IsNullOrEmpty(userEmail))
                return false;
            return !_accountRepository.GetAssistantAccountsFromPrimary(userEmail).Any();
        }

        private static bool IsNotifyEmail1NotEmpty(AccountInformation acctInfo)
        {
            return !string.IsNullOrEmpty(acctInfo.NotifyEmail1);
        }

        private bool IsTheAccountPayedFor(AccountInformation acctInfo)
        {
            var isPayedFor = acctInfo.DatePayedThrough.Date >= DateTime.Now.Date;
            if (!isPayedFor && acctInfo.Autopay)
            {
                _subscriptionCharger.Charge(acctInfo.Id, -1);
                return true;
            }

            return isPayedFor;
        }

        private static bool IsOfficePhoneNotEmpty(AccountInformation acctInfo)
        {
            return !string.IsNullOrEmpty(acctInfo.OfficePhone);
        }

        private static bool IsOfficeNameNotEmpty(AccountInformation acctInfo)
        {
            return !string.IsNullOrEmpty(acctInfo.OfficeName);
        }
    }

    public class AccountCompletionStatus
    {
        public bool PaymentSetupIsComplete { get; set; }
        public bool BasicInfoIsComplete { get; set; }
        public bool NotifyEmailsIsComplete { get; set; }
        public bool AssistantAccountsNeedsSome { get; set; }
        public bool AccountIsUsable { get; set; }
    }
}