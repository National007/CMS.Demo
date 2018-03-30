using System.Web.Mvc;

namespace CMS.MVC.Areas.Com
{
    public class ComAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Com";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "Com_default",
                "Com/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}