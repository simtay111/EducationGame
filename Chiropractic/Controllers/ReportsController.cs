using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using DataLayer;
using DomainLayer.Authentication;
using DomainLayer.Email;
using DomainLayer.Entities;
using DomainLayer.Entities.Quizes;
using EducationGame.Controllers.CustomResults;
using EducationGame.Filters;
using NHibernate.Linq;

namespace EducationGame.Controllers
{
    public class ReportsController : Controller
    {
        [Authorize]
        [AuthorizationFilter(RolesStatic.SuperUser)]
        public ActionResult Index()
        {
            return View();
        }
    }

    public class MemberEducationReport
    {

        public List<MemberRow> MemberRow { get; set; }
        public int TotalQuizzes { get; set; }
    }
    public class MemberRow
    {
        public String MemberName { get; set; }
        public int QuizzesCompleted { get; set; }
        public int Points { get; set; }
    }


    public class ReportModel
    {
        public List<MemberQuizStatus> Quizes { get; set; }

        public List<Member> Members { get; set; }

        public List<AccountInformation> Clinics { get; set; }
    }
}