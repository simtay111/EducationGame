using System;
using DomainLayer.RepoInterfaces;

namespace DomainLayer.Email
{
    public class TwoWeekEmailSender
    {
        private readonly ISystemStateRepository _systemStateRepository;
        private readonly IAccountRepository _accountRepository;
        private readonly IAuditLogRepository _auditLogRepository;


        public TwoWeekEmailSender(ISystemStateRepository systemStateRepository, IAccountRepository accountRepository, IAuditLogRepository auditLogRepository)
        {
            _systemStateRepository = systemStateRepository;
            _accountRepository = accountRepository;
            _auditLogRepository = auditLogRepository;
        }

        public void CheckAndSend()
        {
            var state = _systemStateRepository.Get();
            if (state.LastDateChecked < DateTime.Now.AddDays(-1))
            {
                TraceLog.WriteLine("SENDING CHECK");
                var accounts = _accountRepository.GetAcctInfoThatIsTwoDaysOld();
                var emailSender = new EmailSender(_auditLogRepository);

                foreach (var acct in accounts)
                {
                    TraceLog.WriteLine("Sending Two Day Reminder to: " + acct.OfficeName);
                    if (!string.IsNullOrEmpty(acct.NotifyEmail1))
                        emailSender.SendEmail(acct.NotifyEmail1, GetTwoDayMessage(acct.OfficeName), "Getting started is easy!", acct.NotifyEmail2);
                }

                var threeDayAccounts = _accountRepository.GetAcctInfoThatIsFiveDaysOld();

                foreach (var acct in threeDayAccounts)
                {
                    TraceLog.WriteLine("Sending Five Day Reminder to: " + acct.OfficeName);
                    if (!string.IsNullOrEmpty(acct.NotifyEmail1))
                        emailSender.SendEmail(acct.NotifyEmail1, GetFiveDayMessage(acct.OfficeName), "Hands off education!", acct.NotifyEmail2);
                }
                state.LastDateChecked = DateTime.Now;
                _systemStateRepository.Save(state);
            }
        }

        private string GetTwoDayMessage(string practiceName)
        {
            return
                "Hi " + practiceName + ",\n" +

                "Hopefully, you've started using PracticeOwl by now, since it's SO easy to start!<br><br>Chiropractor's just like you have been registering every day and clearly are recognizing how effortless PracticeOwl removes the burden of educating patients in their offices.\n\n" +

                "If you haven't yet started using it, what are you waiting for?\n\n" +

                "In the time it's taken you to read this email, you could already have patients learning about chiropractic WITHOUT YOU having to lift a finger.\n\n" +

                "Login here to get it going: http://www.PracticeOwl.com \n\n" +

                "To making your life easier,\n\n" +
                "Your Team at PracticeOwl\n ";
        }

        private string GetFiveDayMessage(string practiceName)
        {
            return "Hi " + practiceName + ",\n" +

"This is the last email we're going to send you, motivating you to get started with using PracticeOwl (if you haven't already).\n\n" +

"Remember, just a few minutes is all it takes to SHAVE HOURS off the time you are currently spending educating and selling patients (yuk) on why they should come in to buy your care!\n\n" +

"PracticeOwl takes all that yukkie-stuff off your shoulders and does it for you.\n\n" +

"If patients are already playing, consider this email a HIGH FIVE!\n\n" +

"We appreciate you and hope all of our hard work and sacrifices to make this program a reality has made your life a little easier.\n\n" +

"Your Dedicated Team at PracticeOwl\n";
        }

    }
}