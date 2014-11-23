using System;
using System.Linq;
using System.Web.Mvc;
using DataLayer;
using DomainLayer;
using DomainLayer.Accounts;
using DomainLayer.Authentication;
using DomainLayer.Entities;
using DomainLayer.OrderProcessing;
using DomainLayer.Reports;
using EducationGame.Controllers.CustomResults;
using EducationGame.Filters;

namespace EducationGame.Controllers
{
    public class AccountsController : Controller
    {
        [Authorize]
        [AuthorizationFilter(RolesStatic.SuperUser)]
        public ActionResult Manage()
        {
            return View();
        }

        public JsonDotNetResult GetLogoUrl()
        {
            var url = Constants.ImageBaseUrl + (int)Session[SessionConstants.AcctInfoId] + Constants.ImageExt;
            return new JsonDotNetResult { Data = new { url }, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        [Authorize]
        [AuthorizationFilter(RolesStatic.SuperUser)]
        public JsonDotNetResult GetAssistantAccounts()
        {
            var accountRepo = new AccountRepository(new ConnectionProvider());
            var accounts = accountRepo.GetAssistantAccountsFromPrimary(User.Identity.Name.ToUpper());

            return new JsonDotNetResult
            {
                JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                Data = new
                {
                    accounts
                }
            };
        }

        [Authorize]
        [AuthorizationFilter(RolesStatic.SuperUser)]
        public JsonDotNetResult DeleteAsstAcct(Account acct)
        {
            var accountRepo = new AccountRepository(new ConnectionProvider());
            accountRepo.Delete(acct);
            return new JsonDotNetResult();
        }

        [Authorize]
        public JsonDotNetResult GetAccountInformation()
        {
            var accountRepo = new AccountRepository(new ConnectionProvider());
            var accountInfo = accountRepo.GetAccountInformation(User.Identity.Name);
            return new JsonDotNetResult { Data = accountInfo, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        [HttpPost]
        [Authorize]
        [AuthorizationFilter(RolesStatic.SuperUser)]
        public JsonDotNetResult UpdateAccountInformation(AccountInformation model)
        {
            var accountRepo = new AccountRepository(new ConnectionProvider());
            var existingAccount = accountRepo.GetAcctInfoById(model.Id);
            existingAccount.OfficeName = model.OfficeName;
            existingAccount.NotifyEmail1 = model.NotifyEmail1;
            existingAccount.NotifyEmail2 = model.NotifyEmail2;
            existingAccount.OfficePhone = model.OfficePhone;

            accountRepo.SaveAccountInformation(existingAccount);

            return new JsonDotNetResult();
        }

        [Authorize]
        public JsonDotNetResult GetAccountInfoSummary()
        {
            var acctInfoId = (int) Session[SessionConstants.AcctInfoId];
            var acctId = (int) Session[SessionConstants.AccountId];
            var accountRepo = new AccountRepository(new ConnectionProvider());
            var acct = accountRepo.GetById(acctId);
            var updater = new AccountInfoDataUpdater(accountRepo, new MemberRepository(new ConnectionProvider()));
            var acctInfo = accountRepo.GetAcctInfoById(acctInfoId);

            var summaryBuilder = new SummaryBuilder(accountRepo, updater);
            var nameValue = summaryBuilder.BuildSummary(acctInfoId);

            return new JsonDotNetResult { Data = new { acct, acctInfo, summaries = nameValue }, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        [Authorize]
        public JsonDotNetResult GetAccountCompletionState()
        {
            var userEmail = User.Identity.Name.ToUpper();
            var statusGetter = new AccountCompletionStateGetter(new AccountRepository(new ConnectionProvider()), new SubscriptionCharger(new AccountRepository(new ConnectionProvider()),new ReceiptRepository(new ConnectionProvider()), new PaymentProcessor(new PaymentAuditRepository(new ConnectionProvider()))));
            var status = statusGetter.GetStatus((int)Session[SessionConstants.AcctInfoId], userEmail);
            var resultData = new
                {
                    basic = status.BasicInfoIsComplete,
                    notify = status.NotifyEmailsIsComplete,
                    payment = status.PaymentSetupIsComplete,
                    assistants = status.AssistantAccountsNeedsSome,
                    accountIsDone = status.AccountIsUsable
                };
            return new JsonDotNetResult
                       {
                           Data = resultData,
                           JsonRequestBehavior = JsonRequestBehavior.AllowGet
                       };
        }

        [Authorize]
        public JsonDotNetResult GetLatestHistory()
        {
            var accountRepo = new AccountRepository(new ConnectionProvider());
            var acctInfo = accountRepo.GetAcctInfoById((int)Session[SessionConstants.AcctInfoId]);
            var memberQuizRepo = new MemberQuizStatusRepository(new ConnectionProvider());
            var history =
                memberQuizRepo.GetLatestFiveCompletedQuizHistories(acctInfo.Id, DateTime.Now.AddHours(-8))
                              .OrderByDescending(x => x.DateCompleted)
                              .Take(10);
            return new JsonDotNetResult { Data = history, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
    }
    public class ChangePermissionsModel
    {
        public int Id { get; set; }
        public bool IsManager { get; set; }
        public bool IsAssistant { get; set; }
    }

    public class PermissionsIdName
    {
        public int AccountId { get; set; }
        public bool HasPermission { get; set; }
        public string Name { get; set; }
    }
}
