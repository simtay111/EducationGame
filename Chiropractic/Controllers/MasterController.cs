using System;
using System.Linq;
using System.Web.Mvc;
using DataLayer;
using DomainLayer.Authentication;
using DomainLayer.ECards;
using DomainLayer.Email;
using DomainLayer.Entities;
using DomainLayer.OrderProcessing;
using DomainLayer.Reports;
using EducationGame.Controllers.CustomResults;
using EducationGame.Filters;
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
        public JsonDotNetResult UpdateAcctId(int acctId)
        {
            var accountRepo = new AccountRepository(new ConnectionProvider());

            var current = accountRepo.GetById((int)Session[SessionConstants.AccountId]);

            current.AccountInformation = accountRepo.GetAcctInfoById(acctId);

            accountRepo.Save(current);
            Session[SessionConstants.AcctInfoId] = current.AccountInformation.Id;

            return new JsonDotNetResult();
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
