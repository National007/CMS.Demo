using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CMS.MVC.Controllers
{
    public class DemoController : Controller
    {
        // GET: Demo
        public ActionResult CheckList()
        {
            return View();
        }
        public ActionResult RadioList()
        {
            return View();
        }

    }
}