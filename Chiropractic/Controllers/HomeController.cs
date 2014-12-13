using System.Web.Mvc;
using DataLayer;
using DomainLayer.Email;

namespace EducationGame.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View("UserHome");
        }

        public ActionResult UserHome()
        {
            return View();
        }
    }
}
