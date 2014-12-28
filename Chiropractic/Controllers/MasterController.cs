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
