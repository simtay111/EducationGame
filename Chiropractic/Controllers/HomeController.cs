using System.Web.Mvc;
using Chiropractic;
using DataLayer;
using DomainLayer.DailyTokens;
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

        public ActionResult UpdateDailyToken()
        {
            var updater = new DailyTokenUpdater(new AccountRepository(new ConnectionProvider()));
            updater.Update();
            var sender = new DailyPrintoutSender(new AccountRepository(new ConnectionProvider()), new EmailSender(new AuditLogRepository(new ConnectionProvider())));
            sender.SendDailyPrintout();
            return View();
        }

        [Authorize]
        public ActionResult UserHome()
        {
            var updater = new DailyTokenUpdater(new AccountRepository(new ConnectionProvider()));

            updater.Update();
            return View();
        }

        [Authorize]
        public ActionResult PrintableDaily()
        {
            var acctInfoId = (int)Session[SessionConstants.AcctInfoId];
            var acctInfo = new AccountRepository(new ConnectionProvider()).GetAcctInfoById(acctInfoId);
            var acctPointUpdater = new AccountPointAdder(new AccountRepository(new ConnectionProvider()));
            acctPointUpdater.AddPointsToAccount((int)Session[SessionConstants.AccountId], PointValues.PrintDaily, (int)Session[SessionConstants.AcctInfoId]);
            var model = new PrintableDailyModel
                {
                    DailyCode = (int)acctInfo.DailyToken,
                    ClinicId = acctInfo.Id
                };
            return View(model);
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
