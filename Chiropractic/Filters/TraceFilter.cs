using System;
using System.Web.Mvc;
using DataLayer;
using DomainLayer;
using DomainLayer.Email;
using DomainLayer.Entities;

namespace EducationGame.Filters
{
    public class TraceFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var auditLogRepo = new AuditLogRepository(new ConnectionProvider());
            var user = string.IsNullOrEmpty(filterContext.HttpContext.User.Identity.Name) ? string.Empty : filterContext.HttpContext.User.Identity.Name;
            var newLog = new AuditLog
                {
                    UserId = user,
                    Message = filterContext.HttpContext.Request.RawUrl
                };
            SaveAudit(auditLogRepo, newLog);
     
            TraceLog.WriteLine(user + " " + newLog.Message);

            base.OnActionExecuting(filterContext);
        }

        private static void SaveAudit(AuditLogRepository auditLogRepo, AuditLog newLog)
        {
            try
            {
                auditLogRepo.SaveNew(newLog);
            }
            catch (Exception ex)
            {
                var emailSender = new EmailSender(new AuditLogRepository(new ConnectionProvider()));
                emailSender.SendEmail("Simtay111@gmail.com", ex.StackTrace, "TRACE FILTER");
            }
        }
    }
}