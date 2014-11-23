using System;
using System.Web.Mvc;
using DataLayer;
using DomainLayer.Email;
using DomainLayer.Entities;
using DomainLayer.Points;
using EducationGame.Controllers.CustomResults;

namespace EducationGame.Controllers
{
    public class MembersController : Controller
    {
        [Authorize]
        public ActionResult Index()
        {
            return View();
        }

        //PHI!!!!!!!!!!!!!!!!
        public JsonDotNetResult GetMember(int memberId)
        {
            var member = new MemberRepository(new ConnectionProvider()).GetById(memberId);

            return new JsonDotNetResult { Data = member, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        [HttpPost]
        public JsonDotNetResult SelfAdd(SelfAddModel model)
        {
            var accountInfo = new AccountRepository(new ConnectionProvider()).GetAcctInfoById(model.ClinicId);
            if (accountInfo == null)
                return new JsonDotNetResult
                    {
                        Data = new { success = false, message = "No clinic was found with that id." }
                    };
            if (accountInfo.DailyToken != model.DailyToken)
                return new JsonDotNetResult
                {
                    Data = new { success = false, message = "We found the clinic, but that daily token doesn't match for it! Are both your clinic id and token right?" }
                };
            var memberRepo = new MemberRepository(new ConnectionProvider());
            var member = new Member
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                PhoneNumber = model.PhoneNumber,
                AccountInformation = accountInfo
            };
            memberRepo.Save(member);

            return new JsonDotNetResult { Data = new { success = true } };
        }

        [Authorize]
        public JsonDotNetResult GetMembers(bool includeInactive)
        {
            var accountInfo = new AccountRepository(new ConnectionProvider()).GetAccountInformation(User.Identity.Name);
            var memberRepo = new MemberRepository(new ConnectionProvider());
            var listOfMembers = memberRepo.GetMembersByAccountInfo(accountInfo.Id, includeInactive);
            return new JsonDotNetResult { JsonRequestBehavior = JsonRequestBehavior.AllowGet, Data = listOfMembers };
        }

        [HttpPost]
        [Authorize]
        public JsonDotNetResult AddMember(AddMemberModel model)
        {
            var acctRepo = new AccountRepository(new ConnectionProvider());
            var memberRepo = new MemberRepository(new ConnectionProvider());
            var accountInfoId = (int)Session[SessionConstants.AcctInfoId];
            var accountInfo = acctRepo.GetAcctInfoById(accountInfoId);

            var existingMember = memberRepo.GetMemberByPhoneAndAccountId(model.PhoneNumber, accountInfoId);
            if (existingMember == null)
            {
                var member = new Member
                    {
                        FirstName = model.FirstName,
                        LastName = model.LastName,
                        PhoneNumber = model.PhoneNumber,
                        AccountInformation = accountInfo
                    };
                memberRepo.Save(member);

                if (model.SendSms)
                {
                    try
                    {
                        var smsSender = new SmsMessageSender();
                        var message =
                            string.Format(
                                "Welcome to {0}. Here is today's daily token: {1}  Go to http://www.chiroplay.com to start earning prizes! Make sure to favorite the site!",
                                accountInfo.OfficeName, accountInfo.DailyToken);
                        if (message.Length < 140)
                            smsSender.SendMessage(member.PhoneNumber, message, "");
                        else
                        {
                            smsSender.SendMessage(member.PhoneNumber, message, "Go");
                        }
                    }
                    catch (Exception ex)
                    {

                    }
                }

                var acctPointUpdater = new AccountPointAdder(new AccountRepository(new ConnectionProvider()));
                acctPointUpdater.AddPointsToAccount((int)Session[SessionConstants.AccountId], PointValues.AddMember, (int)Session[SessionConstants.AcctInfoId]);

                return new JsonDotNetResult { Data = new { successful = true } };
            }
            else
            {
                return new JsonDotNetResult { Data = new { successful = false, message = "A member already exists with that phone number. (They may be 'inactive', go to the search screen and check the 'Include inactive members')" } };
            }
        }

        [HttpPost]
        [Authorize]
        public JsonDotNetResult UpdateMember(Member model)
        {
            var memberRepository = new MemberRepository(new ConnectionProvider());

            memberRepository.Save(model);

            return new JsonDotNetResult();
        }

        [HttpPost]
        [Authorize]
        public JsonDotNetResult MarkInactive(Member model)
        {
            var memberRepository = new MemberRepository(new ConnectionProvider());

            memberRepository.ChangeActievState(model.Id, true);

            return new JsonDotNetResult();
        }

        [HttpPost]
        [Authorize]
        public JsonDotNetResult MarkActive(Member model)
        {
            var memberRepository = new MemberRepository(new ConnectionProvider());

            memberRepository.ChangeActievState(model.Id, false);

            return new JsonDotNetResult();
        }

        [Authorize]
        public JsonDotNetResult GetQuizHistory(int memberId)
        {
            var historyRepo = new MemberQuizStatusRepository(new ConnectionProvider());

            return new JsonDotNetResult
                       {
                           Data = historyRepo.GetHistoryForMember(memberId),
                           JsonRequestBehavior = JsonRequestBehavior.AllowGet
                       };
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
