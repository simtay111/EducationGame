using DomainLayer.RepoInterfaces;

namespace DomainLayer.Email
{
    public class DailyPrintoutSender
    {
        private readonly IAccountRepository _accountRepository;
        private readonly EmailSender _emailSender;

        public DailyPrintoutSender(IAccountRepository accountRepository, EmailSender emailSender)
        {
            _accountRepository = accountRepository;
            _emailSender = emailSender;
        }

        public void SendDailyPrintout()
        {
            var accountInfos = _accountRepository.GetAllAccountInformations();

            foreach (var acctInfo in accountInfos)
            {
                if (acctInfo.SentDailyPrintout)
                    continue;
                var accounts = _accountRepository.GetAssistantAccountsForAccountInformation(acctInfo.Id);
                var htmlToSend = @"<div style='text-align: center'><img src='https://www.practiceowl.com/Images/SecretTokenPrintout.jpg'/>
        <h1>" + acctInfo.DailyToken + @"</h1>
        <h3>For New Patients:</h3>
        <h3>Clinic Id: " + acctInfo.Id + "</h3></div>";
                foreach (var account in accounts)
                {
                    _emailSender.SendEmail(account.Email, htmlToSend, "PracticeOwl Daily Prinout", isHtml: true);
                }
                acctInfo.SentDailyPrintout = true;
                _accountRepository.SaveAccountInformation(acctInfo);
            }
        }
    }
}