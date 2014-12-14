using System.Web.Mvc;
using DataLayer;
using DomainLayer.Email;

namespace EducationGame.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View("AcctHome");
            return View("UserHome");
        }

        public ActionResult AcctHome()
        {
            return View();
        }

        public ActionResult UserHome()
        {
            return View();
        }
    }
}
