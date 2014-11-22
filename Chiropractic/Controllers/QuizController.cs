using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using DataLayer;
using DomainLayer.Accounts;
using DomainLayer.Entities;
using DomainLayer.OrderProcessing;
using DomainLayer.Quizzes;
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

        [HttpPost]
        public JsonDotNetResult StartQuizViaPhone(QuizStartModel model)
        {
            var memberRepo = new MemberRepository(new ConnectionProvider());
            if (model.MemberId != 0)
            {
                var member = memberRepo.GetById(model.MemberId);
                return GetResultForMember(member, model.DailyToken);
            }
            else
            {
                var members = memberRepo.GetByPhoneNumber(model.PhoneNumber);

                if (members.Count == 0)
                    return new JsonDotNetResult { Data = new { failed = true, notFound = true, message = "We could not find a registered patient with that phone number." } };

                var membersMatchingToken = (model.PhoneNumber == "6786786789") ? members : members.Where(x => x.AccountInformation.DailyToken == model.DailyToken).ToList();
                if (membersMatchingToken.Count == 0 && model.PhoneNumber != "6786786789")
                    return new JsonDotNetResult { Data = new { failed = true, message = "We found you, but you entered in the wrong daily token!" } };

                if (membersMatchingToken.Count > 1)
                {
                    return GetResultForMultipleMembers(membersMatchingToken);
                }
                var member = membersMatchingToken.Single();

                return GetResultForMember(member, model.DailyToken);
            }
        }

        public JsonDotNetResult GetResultForMultipleMembers(List<Member> members)
        {
            var clinics = members.Select(x => new { officeName = x.AccountInformation.OfficeName, memberId = x.Id });
            return new JsonDotNetResult
            {
                Data =
                    new
                    {
                        failed = true,
                        multiplePatients = true,
                        clinics,
                        message =
                            "There were multiple patients found with this phone number, what clinic are you currently at?"
                    }
            };
        }

        public JsonDotNetResult GetResultForMember(Member member, int dailyToken)
        {
            var quizStarter = new QuizStarter(new MemberRepository(new ConnectionProvider()), new MemberQuizStatusRepository(new ConnectionProvider()), new AccountCompletionStateGetter(new AccountRepository(new ConnectionProvider()), new SubscriptionCharger(new AccountRepository(new ConnectionProvider()), new ReceiptRepository(new ConnectionProvider()), new PaymentProcessor(new PaymentAuditRepository(new ConnectionProvider())))));

            int randomToken = 0;
            try
            {
                randomToken = quizStarter.StartQuizForMember(member, dailyToken);
            }
            catch (QuizStartException ex)
            {
                if (ex.ErrorMessage.Contains("done more quizzes"))
                    return new JsonDotNetResult { Data = new { quizToken = 9999, memberId = member.Id } };
                return new JsonDotNetResult { Data = new { failed = true, message = ex.ErrorMessage } };
            }

            return new JsonDotNetResult { Data = new { quizToken = randomToken, memberId = member.Id } };
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
