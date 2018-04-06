using CMS.MVC.Filter;
using System.Web;
using System.Web.Mvc;

namespace CMS.MVC
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new LogFilter());
            filters.Add(new HandleErrorAttribute());
        }
    }
}
