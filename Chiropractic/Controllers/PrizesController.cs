using System.Web.Mvc;
using DataLayer;
using EducationGame.Controllers.CustomResults;

namespace EducationGame.Controllers
{
    public class PrizesController : Controller
    {
        [Authorize]
        public ActionResult Index()
        {
            return View();
        }
    }
}
