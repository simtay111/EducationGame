using System.Web.Mvc;
using DataLayer;
using DomainLayer.Authentication;
using EducationGame.Controllers.CustomResults;
using EducationGame.Filters;
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
