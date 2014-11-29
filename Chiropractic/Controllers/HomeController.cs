using System.Web.Mvc;
using DataLayer;
using DomainLayer.Email;
using DomainLayer.Points;

namespace EducationGame.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            if (Request.Url.Host == "chiroplay.com" || Request.Url.Host == "localhost" || Request.Url.Host == "www.chiroplay.com")
                return View("WhoAreYou");

            return View("Index2");
        }

        public ActionResult Index2()
        {
            return View();
        }
        public ActionResult Index3()
        {
            return View();
        }

        public ActionResult Index5()
        {
            return View();
        }

        public ActionResult Features()
        {
            return View();
        }

        public ActionResult Pricing()
        {
            return View();
        }

        public ActionResult Faq()
        {
            return View();
        }

        public ActionResult PatientAccess()
        {
            return View("WhoAreYou");
        }


        [Authorize]
        public ActionResult UserHome()
        {
            return View();
        }


        public ActionResult Management()
        {
            return View();
        }
        public ActionResult ContactUs()
        {
            return View();
        }

        [HttpPost]
        public void Contact(ContactModel model)
        {
            var emailSender = new EmailSender(new AuditLogRepository(new ConnectionProvider()));

            emailSender.SendEmail("nevinrosenberg@gmail.com", model.et_pb_contact_message + " from " + model.et_pb_contact_email + " " + model.et_pb_contact_name, "Contact Us Page Was hit");
            emailSender.SendEmail("simtay111@gmail.com", model.et_pb_contact_message + " from " + model.et_pb_contact_email + " " + model.et_pb_contact_name, "Contact Us Page Was hit");
        }

    }

    public class ContactModel
    {
        public string et_pb_contact_name { get; set; }
        public string et_pb_contact_email { get; set; }
        public string et_pb_contact_message { get; set; }
    }

    public class PrintableDailyModel
    {
        public int DailyCode { get; set; }

        public int ClinicId { get; set; }
    }
}
