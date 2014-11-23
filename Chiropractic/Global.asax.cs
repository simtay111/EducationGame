using System;
using System.Configuration;
using System.Security.Principal;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;
using DataLayer.Configuration;
using DomainLayer;
using NHibernate.Context;
using TangoApi;
using WePaySDK;

namespace EducationGame
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);

            WePayConfig.accessToken = ConfigurationManager.AppSettings["WepayAccessToken"];
            WePayConfig.accountId = Convert.ToInt32(ConfigurationManager.AppSettings["WepayAccountId"]);
            WePayConfig.clientId = Convert.ToInt32(ConfigurationManager.AppSettings["WepayClientId"]);
            WePayConfig.clientSecret = ConfigurationManager.AppSettings["WepayClientSecret"];
            WePayConfig.productionMode = Convert.ToBoolean(ConfigurationManager.AppSettings["ProductionMode"]);
            TangoCredentials.Identifier = ConfigurationManager.AppSettings["TangoId"];
            TangoCredentials.Key = ConfigurationManager.AppSettings["TangoKey"];
            TangoCredentials.Endpoint = ConfigurationManager.AppSettings["TangoEndpoint"];

            SystemConfig.DebugMode = !WePayConfig.productionMode;
        }

        protected void Application_PostAuthenticateRequest(Object sender, EventArgs e)
        {
            var authCookie = Context.Request.Cookies[FormsAuthentication.FormsCookieName];
            if (authCookie == null) return;
            var authTicket = FormsAuthentication.Decrypt(authCookie.Value);
            var roles = authTicket.UserData.Split(new[] { ',' });
            var userPrincipal = new GenericPrincipal(new GenericIdentity(authTicket.Name), roles);
            Context.User = userPrincipal;
        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {
            if (!CurrentSessionContext.HasBind(NHibernateHelper.SessionFactory))
            {
                CurrentSessionContext.Bind(NHibernateHelper.OpenSession());
            }
            var session = NHibernateHelper.OpenSession();
            CurrentSessionContext.Bind(session);
        }

        protected void Application_EndRequest(Object sender, EventArgs e)
        {
            if (CurrentSessionContext.HasBind(NHibernateHelper.SessionFactory))
            {
                var session = CurrentSessionContext.Unbind(NHibernateHelper.SessionFactory);
                session.Flush();
                session.Dispose();
            }
        }
    }
}