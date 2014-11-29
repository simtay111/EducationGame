using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
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
