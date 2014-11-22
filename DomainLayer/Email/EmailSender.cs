using System;
using System.Net;
using DomainLayer.Entities;
using DomainLayer.RepoInterfaces;

namespace DomainLayer.Email
{
    public class EmailSender
    {
        private readonly IAuditLogRepository _auditLogRepository;

        public EmailSender(IAuditLogRepository auditLogRepository)
        {
            _auditLogRepository = auditLogRepository;
        }

        public void SendForgotPasswordToken(string username, ForgotPasswordToken forgotPasswordToken)
        {
            var subject = "Forgot Password?";
            var body = "Hello,\n\n We see you forgot your password.  Here is your unique token that you will need to set" + " a new password: " + forgotPasswordToken.UniqueToken + "\n\n Thanks for using PracticeOwl.com";
            SendEmail(username, body, subject);
        }

        public void SendEmail(string username, string body, string subject, string cc = "", bool isHtml = false)
        {
            var message = new System.Net.Mail.MailMessage();
            if (SystemConfig.DebugMode)
            {
                body += username;
                message.To.Add("Simtay111@gmail.com");
            }
            else
            {
                message.To.Add(username);
            }

            message.Subject = subject;
            message.From = new System.Net.Mail.MailAddress("info@practiceowl.com");
            message.Body = body;
            message.IsBodyHtml = isHtml;
            if (!string.IsNullOrEmpty(cc))
                message.CC.Add(cc);
            var smtp = new System.Net.Mail.SmtpClient("pracserv.arvixevps.com")
                {
                    Credentials = new NetworkCredential("info@practiceowl.com", "letmein"),
                };
            smtp.Timeout = 5000;
            try
            {
                smtp.Send(message);
                var newLog = new AuditLog
                    {
                        Message = "Email Sent To:" + username + " " + subject,
                        TimeStamp = DateTime.Now,
                        UserId = "EMAIL"
                    };
                _auditLogRepository.SaveNew(newLog);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                var errorLog = new AuditLog
                    {
                        Message = "FAILED: Email Sent To:" + username + " " + subject,
                        TimeStamp = DateTime.Now,
                        UserId = "EMAIL"
                    };
                _auditLogRepository.SaveNew(errorLog);
            }
        }
    }
}