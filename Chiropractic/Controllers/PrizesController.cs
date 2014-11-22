using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Chiropractic;
using DataLayer;
using DomainLayer.ECards;
using DomainLayer.Email;
using DomainLayer.Entities;
using DomainLayer.OrderProcessing;
using EducationGame.Controllers.CustomResults;
using TangoApi;

namespace EducationGame.Controllers
{
    public class PrizesController : Controller
    {
        [Authorize]
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [Authorize]
        public JsonDotNetResult Update(List<CustomPrize> model)
        {
            var account =
                new AccountRepository(new ConnectionProvider()).GetAcctInfoById(
                    (int)Session[SessionConstants.AcctInfoId]);
            foreach (var prize in model)
            {
                prize.AccountInformation = account;
            }
            var prizeRepo = new PrizeRepository(new ConnectionProvider());

            prizeRepo.UpdateGroup(model);

            return new JsonDotNetResult();
        }

        [Authorize]
        public JsonDotNetResult GetForAccount()
        {
            var accountInformation =
                new AccountRepository(new ConnectionProvider()).GetAccountInformation(User.Identity.Name.ToUpper());
            var prizeRepo = new PrizeRepository(new ConnectionProvider());

            var prizes = prizeRepo.GetForAccount(accountInformation.Id);

            return new JsonDotNetResult { Data = prizes, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        public JsonDotNetResult GetForAccountWithId(int acctId)
        {
            var accountInformation =
                new AccountRepository(new ConnectionProvider()).GetAcctInfoById(acctId);
            var prizeRepo = new PrizeRepository(new ConnectionProvider());

            var prizes = prizeRepo.GetForAccount(accountInformation.Id).Where(x => !string.IsNullOrEmpty(x.Name));
            var publicPrizes = prizeRepo.GetAllPublicPrizes();

            return new JsonDotNetResult { Data = new { prizes, publicPrizes }, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
        [Authorize]
        public JsonDotNetResult GetPublicPrizes()
        {
            var prizeRepo = new PrizeRepository(new ConnectionProvider());

            var publicPrizes = prizeRepo.GetAllPublicPrizes();

            return new JsonDotNetResult { Data = new { publicPrizes }, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        [Authorize]
        public JsonDotNetResult GetAwardedPrizesForMember(int memberId)
        {
            var prizes = new AwardedPrizeRepository(new ConnectionProvider()).GetForMember(memberId);
            return new JsonDotNetResult { JsonRequestBehavior = JsonRequestBehavior.AllowGet, Data = prizes };
        }

        [Authorize]
        [HttpPost]
        public JsonDotNetResult RedeemPrize(AwardedPrize prize)
        {
            var repo = new AwardedPrizeRepository(new ConnectionProvider());
            var dbPrize = repo.GetById(prize.Id);
            dbPrize.Redeemed = true;
            repo.Save(dbPrize);
            return new JsonDotNetResult();
        }
        [Authorize]
        [HttpPost]
        public JsonDotNetResult RefundPrize(AwardedPrize prize)
        {
            var repo = new AwardedPrizeRepository(new ConnectionProvider());
            var memberRepo = new MemberRepository(new ConnectionProvider());
            var acctInfoId = (int) Session[SessionConstants.AcctInfoId];
            var dbPrize = repo.GetById(prize.Id);
            if (acctInfoId != dbPrize.Member.AccountInformation.Id)
                return new JsonDotNetResult();
            
            if (dbPrize != null)
            {
                dbPrize.Member.TotalPoints += dbPrize.PrizePoints;
                memberRepo.Save(dbPrize.Member);
            }
            return new JsonDotNetResult();
        }


        public JsonDotNetResult AccountCanRedeem(int accountId, int prizeId)
        {
            var accountRepository = new AccountRepository(new ConnectionProvider());
            var prizeRepo = new PrizeRepository(new ConnectionProvider());
            var disclaimers = new RewardDisclaimerRepository(new ConnectionProvider()).GetAll();
            var account = accountRepository.GetById(accountId);
            IPrize prize = null;
            prize = prizeRepo.GetAllPublicPrizes().Single(x => x.Id == prizeId);

            var disclaimer = disclaimers.SingleOrDefault(x => x.Sku == prize.Sku);

            return new JsonDotNetResult
            {
                Data = new { currentPoints = account.Points, canRedeem = account.Points >= prize.Points, disclaimer },
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        public JsonDotNetResult CanRedeem(int memberId, int prizeId, bool preferred)
        {
            var memberRepo = new MemberRepository(new ConnectionProvider());
            var prizeRepo = new PrizeRepository(new ConnectionProvider());
            var disclaimers = new RewardDisclaimerRepository(new ConnectionProvider()).GetAll();
            var member = memberRepo.GetById(memberId);
            IPrize prize = null;
            if (preferred)
            {
                prize = prizeRepo.GetForAccount(member.AccountInformation.Id).Single(x => x.Id == prizeId);
            }
            else
            {
                prize = prizeRepo.GetAllPublicPrizes().Single(x => x.Id == prizeId);
            }

            var disclaimer = disclaimers.SingleOrDefault(x => x.Sku == prize.Sku);

            return new JsonDotNetResult
                {
                    Data = new { currentPoints = member.TotalPoints, canRedeem = member.TotalPoints >= prize.Points, disclaimer },
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };
        }

        [HttpPost]
        public JsonDotNetResult OrderPrize(OrderPrizeModel model)
        {
            var memberRepo = new MemberRepository(new ConnectionProvider());
            var prizeRepo = new PrizeRepository(new ConnectionProvider());
            var awardedPrizeRepo = new AwardedPrizeRepository(new ConnectionProvider());

            var member = memberRepo.GetById(model.MemberId);
            IPrize prize;
            if (model.Preferred)
            {
                prize = prizeRepo.GetForAccount(member.AccountInformation.Id).Single(x => x.Id == model.PrizeId);
            }
            else
            {
                prize = prizeRepo.GetAllPublicPrizes().Single(x => x.Id == model.PrizeId);
            }
            if (member.TotalPoints < prize.Points)
                return new JsonDotNetResult() {Data = new {error= "You don't have enough points for this!"}};
            if (member.AccountInformation.CreditCardToken < 1)
                return new JsonDotNetResult() {Data = new {error= "The clinic you are at has cancelled their service :("}};

            var random = new Random();
            var awardedPrize = new AwardedPrize
                {
                    ConfirmationCode = random.Next(10000, 100000).ToString(),
                    IssueDate = DateTime.Now,
                    Member = member,
                    PrizeName = prize.Name,
                    PrizeSku = prize.Sku,
                    PrizePoints = prize.Points,
                    Redeemed = false,
                    AccountInformation = member.AccountInformation,
                    Ordered = false,
                    DateOrdered = DateTime.Now.AddYears(-100)
                };
            member.TotalPoints = member.TotalPoints - prize.Points;

            memberRepo.Save(member);
            awardedPrizeRepo.Save(awardedPrize);

            if (!model.Preferred)
            {
                var orderRequest = new ECardOrderRequest
                    {
                        AccountInformation = member.AccountInformation,
                        Amount = prize.Points,
                        RecipientEmail = model.Email,
                        RecipientName = member.FirstName + " " + member.LastName,
                        Sku = prize.Sku,
                        AwardedPrizeId = awardedPrize.Id,
                        IsRangePrize = prize.IsRange
                    };

                var cardOrderer = new ECardOrderer(new TangoAcctInfoProvider(), new OrderPlacer(new ServiceProxy()),
                                                   new AwardedPrizeRepository(new ConnectionProvider()),
                                                   new TangoAuditRepository(new ConnectionProvider()));
                cardOrderer.PlaceOrder(orderRequest);

                var awardCharger = new AwardCharger(new AwardedPrizeRepository(new ConnectionProvider()),
                                                    new ReceiptRepository(new ConnectionProvider()),
                                                    new PaymentProcessor(
                                                        new PaymentAuditRepository(new ConnectionProvider())));
                awardCharger.ChargeForSingleAward(awardedPrize);
            }
            else
            {
                try
                {
                    var emailSender = new EmailSender(new AuditLogRepository(new ConnectionProvider()));
                    const string body =
                        "Hello, you have selected a prize that your office is providing.  Please see the front desk about" +
                        " receiving it! Thanks! ";
                    emailSender.SendEmail(model.Email, body, "Reward from " + member.AccountInformation.OfficeName);
                }
                catch (Exception ex)
                {

                }
            }

            return new JsonDotNetResult();
        }
        [HttpPost]
        public JsonDotNetResult OrderPrizeForAccount(OrderPrizeForAccountModel model)
        {
            var accountRepo = new AccountRepository(new ConnectionProvider());
            var prizeRepo = new PrizeRepository(new ConnectionProvider());
            var awardedPrizeRepo = new AccountPrizeRepository(new ConnectionProvider());

            var account = accountRepo.GetById(model.AccountId);
            var prize = prizeRepo.GetAllPublicPrizes().Single(x => x.Id == model.PrizeId);
            if (account.Points < prize.Points)
                return new JsonDotNetResult();

            var awardedPrize = new AccountPrize
            {
                IssueDate = DateTime.Now,
                PrizeName = prize.Name,
                PrizeSku = prize.Sku,
                PrizePoints = prize.Points,
                TargetEmail = model.Email,
                AccountId = (int)Session[SessionConstants.AccountId],
                AccountInformation = account.AccountInformation,
            };
            account.Points = account.Points - prize.Points;

            accountRepo.Save(account);
            awardedPrizeRepo.Save(awardedPrize);

            var orderRequest = new ECardOrderRequest
            {
                AccountInformation = account.AccountInformation,
                Amount = prize.Points,
                RecipientEmail = model.Email,
                RecipientName = account.DisplayName,
                Sku = prize.Sku,
                AwardedPrizeId = awardedPrize.Id,
                IsRangePrize = prize.IsRange
            };

            var cardOrderer = new ECardOrderer(new TangoAcctInfoProvider(), new OrderPlacer(new ServiceProxy()),
                                               new AwardedPrizeRepository(new ConnectionProvider()),
                                               new TangoAuditRepository(new ConnectionProvider()));
            cardOrderer.PlaceOrder(orderRequest, true);

            return new JsonDotNetResult();
        }
    }
    public class OrderPrizeForAccountModel
    {
        public int AccountId { get; set; }
        public int PrizeId { get; set; }
        public string Email { get; set; }
    }

    public class OrderPrizeModel
    {
        public int MemberId { get; set; }
        public int PrizeId { get; set; }
        public bool Preferred { get; set; }
        public string Email { get; set; }
    }
}
