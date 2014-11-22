using DomainLayer.RepoInterfaces;

namespace DomainLayer.Email
{
    public class DocStatReportEmailSender
    {
        private readonly EmailSender _emailSender;
        private readonly IAccountRepository _accountRepository;

        public DocStatReportEmailSender(EmailSender emailSender, IAccountRepository accountRepository)
        {
            _emailSender = emailSender;
            _accountRepository = accountRepository;
        }

        public void CheckAndSend()
        {
            var allAccts = _accountRepository.GetAllAccountInformations();
            foreach (var accountInformation in allAccts)
            {
              //_emailSender.SendEmail("nevinrosenberg@gmail.com", GetMessage(accountInformation), "Clinic Performance Report", "edpfox@gmail.com" );
            }
        }
    }
}