using System.Web.Mvc;

namespace EducationGame.Controllers
{
    public class TutorialController : Controller
    {
        [Authorize]
        public ActionResult Index()
        {
            return View();
        }

        [Authorize]
        public ActionResult Advanced()
        {
            return View();
        }
         
    }
}