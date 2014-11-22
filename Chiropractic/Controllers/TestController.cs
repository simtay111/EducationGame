using System.Web.Mvc;
using DataLayer;
using DomainLayer.Email;

namespace EducationGame.Controllers
{
    public class TestController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public void Send()
        {
            var emailSender = new EmailSender(new AuditLogRepository(new ConnectionProvider()));
            emailSender.SendEmail("Simtay111@gmail.com", "<img src='https://www.practiceowl.com/Images/SecretTokenPrintout.jpg'/>", "test", isHtml:true);
        }
    }
}