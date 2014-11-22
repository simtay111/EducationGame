using System;
using System.Web.Mvc;
using DataLayer;
using DomainLayer;
using DomainLayer.Email;
using DomainLayer.Entities;

namespace Chiropractic.Filters
{
    public class ErrorFilter : HandleErrorAttribute
    {
        public override void OnException(ExceptionContext filterContext)
        {
            var exception = filterContext.Exception;
            var emailSender = new EmailSender(new AuditLogRepository(new ConnectionProvider()));
            var auditLogRepo = new AuditLogRepository(new ConnectionProvider());
            var user = string.IsNullOrEmpty(filterContext.HttpContext.User.Identity.Name) ? string.Empty : filterContext.HttpContext.User.Identity.Name;
            var newLog = new AuditLog
                {
                    UserId = user,
                    Message = exception.Message + "\n" + exception.StackTrace,
                    TimeStamp = DateTime.Now
                };
            try
            {
                emailSender.SendEmail("Simtay111@gmail.com", exception.Message + "\n" + exception.StackTrace, "PRE AUDIT ERROR");
                auditLogRepo.SaveNew(newLog);
            }
            catch (Exception ex)
            {
                emailSender.SendEmail("Simtay111@gmail.com", ex.Message + "\n" + ex.StackTrace, "AUDIT FAILED ERROR");
            }
            TraceLog.WriteLine(newLog.UserId + " " + newLog.Message);

            base.OnException(filterContext);
        }
    }
}