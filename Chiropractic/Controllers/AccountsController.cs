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
            var url = SystemConstants.ImageBaseUrl + (int)Session[SessionConstants.AcctInfoId] + SystemConstants.ImageExt;
            return new JsonDotNetResult { Data = new { url }, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
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
            existingAccount.CompanyName = model.CompanyName;

            accountRepo.SaveAccountInformation(existingAccount);

            return new JsonDotNetResult();
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
