using System;
using System.Web.Mvc;
using DataLayer;
using DomainLayer.Email;
using DomainLayer.OrderProcessing;
using EducationGame.Controllers.CustomResults;
using WePaySDK;

namespace EducationGame.Controllers
{
    public class CreditCardController : Controller
    {
        [HttpPost]
        [Authorize]
        public JsonDotNetResult AddCreditCardToAccount(AddCreditCardModel model)
        {
            var acctInfoId = (int)Session[SessionConstants.AcctInfoId];

            var usePromoCost = false;
            var promoCost = 0;
            if (!string.IsNullOrEmpty(model.PromoCode) && model.PromoCode.Length >= 6)
            {
                var first = model.PromoCode.Substring(0, 1);
                var last = model.PromoCode.Substring(model.PromoCode.Length - 1, 1);
                usePromoCost = int.TryParse(first + last, out promoCost);
            }

            var acctInfoRepo = new AccountRepository(new ConnectionProvider());
            var acctInfo = acctInfoRepo.GetAcctInfoById(acctInfoId);
            acctInfo.CreditCardToken = model.Token;
            var subscriptionCost = 141;
            if (model.Price == 2)
                subscriptionCost = 97;
            if (model.Price == 3)
                subscriptionCost = 997;
            acctInfo.SubscriptionCost = usePromoCost ? promoCost : subscriptionCost;
            acctInfoRepo.SaveAccountInformation(acctInfo);

            var authorizeRequest = new CreditCardAuthorizeRequest();
            authorizeRequest.accessToken = WePayConfig.accessToken;
            authorizeRequest.client_id = WePayConfig.clientId;
            authorizeRequest.client_secret = WePayConfig.clientSecret;
            authorizeRequest.credit_card_id = model.Token;

            var response = new CreditCard().Authorize(authorizeRequest);
            if (response.Error != null)
            {
                var message = String.Format("Failed to authorize CC with id: {0} for account {1} with id {2}",
                                            model.Token, acctInfo.OfficeName, acctInfo.Id);
                acctInfo.CreditCardToken = 0;
                acctInfoRepo.SaveAccountInformation(acctInfo);
                var emailSender = new EmailSender(new AuditLogRepository(new ConnectionProvider()));
                emailSender.SendEmail("Simtay111@gmail.com", message, "CREDIT CARD AUTH FAILURE");
                return new JsonDotNetResult { Data = new { error = "We failed to authorize your credit card.  (IE Wrong zip, name on card does not match etc..)" } };
            }

            if (acctInfo.DatePayedThrough.Date < DateTime.Now.Date)
            {

                var subscriptionProcessor = new SubscriptionCharger(new AccountRepository(new ConnectionProvider()),
                                                                    new ReceiptRepository(new ConnectionProvider()),
                                                                    new PaymentProcessor(
                                                                        new PaymentAuditRepository(
                                                                            new ConnectionProvider())));
                subscriptionProcessor.Charge(acctInfoId, acctInfo.SubscriptionCost);
                if (!acctInfo.PayedOnce)
                {
                    acctInfo.PayedOnce = true;
                    acctInfoRepo.SaveAccountInformation(acctInfo);
                }
                if (acctInfo.SubscriptionCost == 141)
                {
                    acctInfo.SubscriptionCost = 97;
                    acctInfoRepo.SaveAccountInformation(acctInfo);
                }
            }


            return new JsonDotNetResult { Data = new { costCharged = acctInfo.SubscriptionCost } };
        }

        [HttpPost]
        [Authorize]
        public JsonDotNetResult UpdateAutoRenew(bool autoRenew)
        {
            var acctRepo = new AccountRepository(new ConnectionProvider());
            var acctInfo = acctRepo.GetAcctInfoById((int)Session[SessionConstants.AcctInfoId]);

            acctInfo.Autopay = autoRenew;
            acctRepo.SaveAccountInformation(acctInfo);
            return new JsonDotNetResult();
        }

        [Authorize]
        public JsonDotNetResult Status()
        {
            var acctRepo = new AccountRepository(new ConnectionProvider());
            var acctInfo = acctRepo.GetAcctInfoById((int)Session[SessionConstants.AcctInfoId]);

            var hasCc = acctInfo.CreditCardToken > 0;
            var cost = acctInfo.CostPerQuiz.ToString().Replace("0", "");
            var price = acctInfo.SubscriptionCost == 97 ? 1 : 2;
            var subscriptionThrough = acctInfo.DatePayedThrough;
            var payedOnce = acctInfo.PayedOnce;

            return new JsonDotNetResult { JsonRequestBehavior = JsonRequestBehavior.AllowGet, Data = new { hasCcOnFile = hasCc, cost, autoRenew = acctInfo.Autopay, price, subscriptionThrough, payedOnce } };
        }

        [HttpPost]
        [Authorize]
        public JsonDotNetResult BillClinic()
        {
            var acctRepo = new AccountRepository(new ConnectionProvider());
            var acctInfo = acctRepo.GetAcctInfoById((int)Session[SessionConstants.AcctInfoId]);

            if (acctInfo.DatePayedThrough.Date < DateTime.Now.Date)
            {

                var subscriptionProcessor = new SubscriptionCharger(new AccountRepository(new ConnectionProvider()),
                                                                    new ReceiptRepository(new ConnectionProvider()),
                                                                    new PaymentProcessor(
                                                                        new PaymentAuditRepository(
                                                                            new ConnectionProvider())));
                subscriptionProcessor.Charge(acctInfo.Id, acctInfo.SubscriptionCost);
                return new JsonDotNetResult { Data = new { message = "We billed your account: " + acctInfo.SubscriptionCost + ". Thank you for using PracticeOwl!" } };
            }
            return new JsonDotNetResult
            {
                Data = new
                {
                    message = "Your current subscription is already up to date. Please use autorenew if you wish to automatically " +
                            "be resubscribed when your current subscription ends."
                }
            };
        }

        [HttpPost]
        [Authorize]
        public JsonDotNetResult UpdateSubscription(int price)
        {
            var acctRepo = new AccountRepository(new ConnectionProvider());
            var acctInfo = acctRepo.GetAcctInfoById((int)Session[SessionConstants.AcctInfoId]);

            acctInfo.SubscriptionCost = price == 1 ? 97 : 997;
            acctRepo.SaveAccountInformation(acctInfo);

            return new JsonDotNetResult();
        }

        [Authorize]
        public JsonDotNetResult Delete()
        {
            var acctRepo = new AccountRepository(new ConnectionProvider());
            var acctInfo = acctRepo.GetAcctInfoById((int)Session[SessionConstants.AcctInfoId]);

            var paymentProcessor = new PaymentProcessor(new PaymentAuditRepository(new ConnectionProvider()));
            paymentProcessor.DeleteCcCard(acctInfo.CreditCardToken);

            acctInfo.CreditCardToken = 0;
            acctRepo.SaveAccountInformation(acctInfo);

            return new JsonDotNetResult();
        }
    }

    public class AddCreditCardModel
    {
        public long Token { get; set; }
        public string PromoCode { get; set; }
        public int Price { get; set; }
    }
}
