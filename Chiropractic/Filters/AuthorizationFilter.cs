using System.Security.Authentication;
using System.Web.Mvc;
using System.Web.Routing;
using DataLayer.Configuration;

namespace Chiropractic.Filters
{
    /// <summary>Use this filter for read-only actions.</summary>
    public class AuthorizationFilter : ActionFilterAttribute
    {
        public int RequiredPermissionLevel { get; set; }
        public AuthorizationFilter(int requiredPermissionLevel)
        {
            RequiredPermissionLevel = requiredPermissionLevel;
        }
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var currentPermlevel = (int)filterContext.HttpContext.Session[SessionConstants.AcctPermissionLevel];

            if (currentPermlevel < RequiredPermissionLevel)
            {
                filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary 
                { 
                    { "controller", "Login" }
                });
            }
        }

        public override void OnResultExecuted(ResultExecutedContext filterContext)
        {
        }
    }
}