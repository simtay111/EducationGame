using System.Web.Mvc;
using EducationGame.Filters;

namespace EducationGame
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