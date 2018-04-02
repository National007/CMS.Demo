using Serviece.Interface;
using WebModels.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CMS.MVC.Controllers
{
    public class HomeController : BaseController
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Main()
        {
            return View();
        }

        public ActionResult NewsList()
        {
            return View();
        }

        public JsonResult GetList()
        {
            var list = Resolve<IUserServiece>().GetList();
            var layuiGrid = new LayuiGrid();
            layuiGrid.count = list.Count();
            layuiGrid.data = list;
            return Json(layuiGrid,JsonRequestBehavior.AllowGet);
        }

    }
}
