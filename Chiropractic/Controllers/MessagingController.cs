using System.Web.Mvc;
using DataLayer;
using DomainLayer.Accounts;
using DomainLayer.Email;
using DomainLayer.OrderProcessing;
using DomainLayer.Quizzes;
using EducationGame.Controllers.CustomResults;

namespace EducationGame.Controllers
{
    public class MessagingController : Controller
    {
        [HttpPost]
        [Authorize]
        public JsonDotNetResult SendGameUrl(int memberId)
        {
            var memberRepo = new MemberRepository(new ConnectionProvider());
            var member = memberRepo.GetById(memberId);
            var smsSender = new SmsMessageSender();
            var accountInformation =
                new AccountRepository(new ConnectionProvider()).GetAcctInfoById(
                    (int)Session[SessionConstants.AcctInfoId]);

            var randomToken = 0;

            try
            {
                var quizStarter = new QuizStarter(new MemberRepository(new ConnectionProvider()),
                                                  new MemberQuizStatusRepository(new ConnectionProvider()),
                                                  new AccountCompletionStateGetter(
                                                      new AccountRepository(new ConnectionProvider()),
                                                      new SubscriptionCharger(
                                                          new AccountRepository(new ConnectionProvider()),
                                                          new ReceiptRepository(new ConnectionProvider()),
                                                          new PaymentProcessor(
                                                              new PaymentAuditRepository(new ConnectionProvider())))));

                randomToken = quizStarter.StartQuizForMember(member, (int)accountInformation.DailyToken);
            }
            catch (QuizStartException ex)
            {
                return new JsonDotNetResult{Data = new {error = ex.ErrorMessage}};
            }

            var url = @"http://www.chiroplay.com/Quiz?generatedId=" + randomToken + "&memberId=" + member.Id + "#/storyPage";
            smsSender.SendMessage(member.PhoneNumber, "Press here to start a game: " + url, "");
            return new JsonDotNetResult();
        }
    }
}