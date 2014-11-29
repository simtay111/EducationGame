using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using DataLayer;
using DomainLayer.Accounts;
using DomainLayer.Entities;
using DomainLayer.OrderProcessing;
using EducationGame.Controllers.CustomResults;

namespace EducationGame.Controllers
{
    public class QuizController : Controller
    {
        public ActionResult Index(int generatedId, int memberId)
        {
            var model = new QuizStartModel { Token = generatedId, MemberId = memberId };
            return View(model);
        }
    }

    public class QuizStartModel
    {
        public int Token { get; set; }

        public int MemberId { get; set; }
        public int DailyToken { get; set; }
        public string PhoneNumber { get; set; }
    }
}
