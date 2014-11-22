using System;
using System.Linq;
using System.Web.Mvc;
using Chiropractic;
using Chiropractic.Filters;
using DataLayer;
using DomainLayer.Authentication;
using DomainLayer.ECards;
using DomainLayer.Email;
using DomainLayer.Entities;
using DomainLayer.OrderProcessing;
using DomainLayer.Reports;
using EducationGame.Controllers.CustomResults;
using TangoApi;
using TangoApi.Entity;

namespace EducationGame.Controllers
{
    [AuthorizationFilter(RolesStatic.LlcAdmin)]
    [Authorize]
    public class MasterController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public JsonDotNetResult ActivateAllAccounts()
        {
            var accountRepo = new AccountRepository(new ConnectionProvider());
            accountRepo.ActivateAllAccounts();
            return new JsonDotNetResult { JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        public JsonDotNetResult AwardsOrderReport()
        {
            var awardsRepo = new AwardedPrizeRepository(new ConnectionProvider());
            var awards = awardsRepo.GetNonBilledRedeemedTangoAwards().GroupBy(x => x.DateOrdered.Date);
            return new JsonDotNetResult { JsonRequestBehavior = JsonRequestBehavior.AllowGet, Data = awards };
        }

        public JsonDotNetResult GetCurrentAwards()
        {
            var prizeRepo = new PrizeRepository(new ConnectionProvider());
            var items = prizeRepo.GetAllPublicPrizes();
            return new JsonDotNetResult { Data = items, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        public JsonDotNetResult GetAwardChoices()
        {
            var fetcher = new AvailableItemFetcher(new ServiceProxy());
            var items = fetcher.Fetch();
            return new JsonDotNetResult { Data = items, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        [HttpPost]
        public JsonDotNetResult RemoveAward(RemoveRewardRequest request)
        {
            var prizeRepo = new PrizeRepository(new ConnectionProvider());
            prizeRepo.DeleteAvailable(request.Id);
            return new JsonDotNetResult();
        }

        [HttpPost]
        public JsonDotNetResult AddAward(AddRewardRequest request)
        {
            var prizeRepo = new PrizeRepository(new ConnectionProvider());
            var prize = new AvailablePrize
                {
                    ImageUrl = request.Item.image_url,
                    Cost = (request.Reward.unit_price > 0) ? (int)request.Reward.unit_price : 500,
                    Name = request.Item.description,
                    Points = (request.Reward.unit_price > 0) ? (int)request.Reward.unit_price : 500,
                    Sku = request.Reward.sku,
                    IsRange = request.Reward.unit_price == -1
                };
            prizeRepo.Save(prize);

            return new JsonDotNetResult();
        }

        [HttpPost]
        public JsonDotNetResult OrderGames()
        {
            //var receiptRepo = new ReceiptRepository(new ConnectionProvider());
            //var paymentProcessor = new PaymentProcessor(new PaymentAuditRepository(new ConnectionProvider()));
            //var memberQuizStatusRepo = new MemberQuizStatusRepository(new ConnectionProvider());
            //var awardCharger = new QuizCharger(receiptRepo, memberQuizStatusRepo, paymentProcessor);
            //awardCharger.Charge();
            return new JsonDotNetResult();

        }

        [HttpPost]
        public JsonDotNetResult SendReceipts()
        {
            var receiptRepo = new ReceiptRepository(new ConnectionProvider());

            var nonSentReceipts = receiptRepo.GetNonSentReceipt();
            var receiptSender = new ReceiptSender(new EmailSender(new AuditLogRepository(new ConnectionProvider())),
                                                  new SummaryBuilder(new AccountRepository(new ConnectionProvider()),
                                                                     new AccountInfoDataUpdater(
                                                                         new AccountRepository(new ConnectionProvider()),
                                                                         new MemberRepository(new ConnectionProvider()))));
            Console.WriteLine("Receipts to Send: " + nonSentReceipts.Count());
            foreach (var nonSentReceipt in nonSentReceipts)
            {
                var cc = string.IsNullOrEmpty(nonSentReceipt.AccountInformation.NotifyEmail2)
                             ? ""
                             : nonSentReceipt.AccountInformation.NotifyEmail2;
                Console.WriteLine("Sent receipt: " + nonSentReceipt.AccountInformation.NotifyEmail1);
                receiptSender.SendReceipt(nonSentReceipt.AccountInformation.NotifyEmail1, nonSentReceipt, cc);
                receiptRepo.Save(nonSentReceipt);
            }

            return new JsonDotNetResult();
        }

        [HttpPost]
        public JsonDotNetResult UpdateAcctId(int acctId)
        {
            var accountRepo = new AccountRepository(new ConnectionProvider());

            var current = accountRepo.GetById((int)Session[SessionConstants.AccountId]);

            current.AccountInformation = accountRepo.GetAcctInfoById(acctId);

            accountRepo.Save(current);
            Session[SessionConstants.AcctInfoId] = current.AccountInformation.Id;

            return new JsonDotNetResult();
        }

        [HttpPost]
        public JsonDotNetResult OrderAwards()
        {
            var awardsRepo = new AwardedPrizeRepository(new ConnectionProvider());
            var receiptRepo = new ReceiptRepository(new ConnectionProvider());
            var paymentProcessor = new PaymentProcessor(new PaymentAuditRepository(new ConnectionProvider()));
            var gameCharger = new AwardCharger(awardsRepo, receiptRepo, paymentProcessor);
            gameCharger.Charge();

            return new JsonDotNetResult();
        }

        public JsonDotNetResult OrderHistory(DateTime start, DateTime end)
        {
            var tangoProvider = new TangoAcctInfoProvider();
            var request = new OrderHistoryRequestData
                {
                    account_identifier = tangoProvider.GetAcctInfo(),
                    customer = tangoProvider.GetCustomer(),
                    offset = 0,
                    limit = 100,
                    start_date = start.Date,
                    end_date = end.Date.AddHours(24)
                };

            var orderHistoryFetcher = new OrderHistoryFetcher(new ServiceProxy());

            var response = orderHistoryFetcher.GetCurrentStatus(request);

            return new JsonDotNetResult { Data = response, JsonRequestBehavior = JsonRequestBehavior.AllowGet };

        }
    }


    public class RemoveRewardRequest
    {
        public int Id { get; set; }
    }

    public class AddRewardRequest
    {
        public Reward Reward { get; set; }
        public AvailableItem Item { get; set; }
    }
}
