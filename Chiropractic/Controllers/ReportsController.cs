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

        [Authorize]
        public JsonDotNetResult MemberCostReport(DateTime start, DateTime end)
        {
            var memberQuizStatusRepo = new MemberQuizStatusRepository(new ConnectionProvider());
            var acctInfoId = (int)Session[SessionConstants.AcctInfoId];
            var quizzes = memberQuizStatusRepo.GetInRange(acctInfoId, start, end);

            var groupedQuizzes = quizzes.GroupBy(x => x.DateCompleted.Date);

            return new JsonDotNetResult { Data = groupedQuizzes, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        [Authorize]
        public JsonDotNetResult ReceiptReport(DateTime start, DateTime end)
        {
            var receiptRepository = new ReceiptRepository(new ConnectionProvider());
            var acctInfoId = (int)Session[SessionConstants.AcctInfoId];
            var receipts = receiptRepository.GetInRange(acctInfoId, start, end);

            var groupedQuizzes = receipts.GroupBy(x => x.DateBilled.Date);

            return new JsonDotNetResult { Data = groupedQuizzes, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        [Authorize]
        public JsonDotNetResult AwardedPrizeReport(DateTime start, DateTime end)
        {
            var awardedPrizeRepository = new AwardedPrizeRepository(new ConnectionProvider());
            var acctInfoId = (int)Session[SessionConstants.AcctInfoId];
            var awards = awardedPrizeRepository.GetInRange(acctInfoId, start, end);
            var groupedAwards = awards.GroupBy(x => x.IssueDate.Date);
            return new JsonDotNetResult { Data = groupedAwards, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        [Authorize]
        public JsonDotNetResult MemberEducationReport()
        {
            var memberRepository = new MemberRepository(new ConnectionProvider());
            var memberQuizRepo = new MemberQuizStatusRepository(new ConnectionProvider());
            var storyRepo = new StoryRepository(new ConnectionProvider());
            var acctInfoId = (int)Session[SessionConstants.AcctInfoId];
            var storiesForAcct = storyRepo.GetAllForAcctInfo(acctInfoId);


            var members = memberRepository.GetMembersByAccountInfo(acctInfoId, false);

            var memberRows = new List<MemberRow>();
            foreach (var member in members)
            {
                var quizes = memberQuizRepo.GetHistoryForMember(member.Id);
                var memberRow = new MemberRow
                    {
                        MemberName = member.LastName + ", " + member.FirstName,
                        Points = member.TotalPoints,
                        QuizzesCompleted = quizes.Count
                    };
                if (quizes.Any(x => x.DateCompleted > DateTime.Now.AddDays(-7)))
                    memberRows.Add(memberRow);
            }
            var report = new MemberEducationReport { MemberRow = memberRows, TotalQuizzes = storiesForAcct.Count };

            return new JsonDotNetResult { JsonRequestBehavior = JsonRequestBehavior.AllowGet, Data = report };
        }

        public ActionResult GetReport()
        {
            var connectionProvider = new ConnectionProvider();
            List<MemberQuizStatus> theQuizes;
            var connection = connectionProvider.CreateConnection();
            theQuizes = (from stats in connection.Query<MemberQuizStatus>() where stats.DateCompleted > DateTime.Now.AddDays(-1) select stats).OrderByDescending(x => x.DateCompleted).ToList();
            var accts = new AccountRepository(connectionProvider).GetAllAccountInformations();

            var reportModel = new ReportModel
                {
                    Quizes = theQuizes.ToList(),
                    Clinics = accts
                };

            var twoWeekReminders = new TwoWeekEmailSender(new SystemStateRepository(new ConnectionProvider()), new AccountRepository(new ConnectionProvider()), new AuditLogRepository(new ConnectionProvider()));
            twoWeekReminders.CheckAndSend();
            return View(reportModel);
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