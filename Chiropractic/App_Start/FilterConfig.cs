using System.Web;
using System.Web.Mvc;
using Chiropractic.Filters;

namespace Chiropractic
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new TraceFilter());
            filters.Add(new ErrorFilter());
            filters.Add(new SessionDataFilter());
        }
    }
}