using System.Web.Mvc;

namespace EducationGame.Controllers
{
    public class MembersController : Controller
    {
        [Authorize]
        public ActionResult Index()
        {
            return View();
        }
    }

    public class SelfAddModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int ClinicId { get; set; }
        public int DailyToken { get; set; }
        public string PhoneNumber { get; set; }
    }

    public class GetHistoryModel
    {
        public int MemberId { get; set; }
    }
    public class AddMemberModel
    {
        public string FirstName { get; set; }
        public string PhoneNumber { get; set; }
        public string LastName { get; set; }
        public bool SendSms { get; set; }
    }
}
