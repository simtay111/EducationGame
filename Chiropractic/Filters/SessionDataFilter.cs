﻿using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using DataLayer;
using DomainLayer;
using DomainLayer.Entities;

namespace EducationGame.Filters
{
    public class SessionDataFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var context = filterContext.RequestContext.HttpContext;
            if (RequestIsAuthenticatedButSessionFieldsAreNotFull(context))
            {
                ReapplySessionData(context);
            }
        }

        private static void ReapplySessionData(HttpContextBase context)
        {
            TraceLog.WriteLine("Resetting Session Data");
            var accountRepo = new AccountRepository(new ConnectionProvider());
            var memberRepo = new MemberRepository(new ConnectionProvider());
            var acct = accountRepo.GetByLoginEmail(context.User.Identity.Name.ToUpper());
            if (acct == null)
                acct = memberRepo.GetByLoginEmail(context.User.Identity.Name.ToUpper());

            if (acct == null)
                FormsAuthentication.SignOut();
            if (context.Session != null && acct != null)
            {
                var seetter = new SessionDataSetter();
                seetter.SetOnSession(context.Session, acct);
            }
        }

        private static bool RequestIsAuthenticatedButSessionFieldsAreNotFull(HttpContextBase context)
        {
            return context.Request.IsAuthenticated && (context.Session[SessionConstants.AccountId] == null);
        }
    }
    public class SessionDataSetter
    {
        public void SetOnSession(HttpSessionStateBase session, IHaveAuthorizationCredentials acct)
        {
            session.Add(SessionConstants.AccountId, acct.Id);
        }
    }
}