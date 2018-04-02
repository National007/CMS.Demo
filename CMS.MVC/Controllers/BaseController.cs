
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;

namespace CMS.MVC.Controllers
{
    public class BaseController : Controller
    {
        #region private
        private HtmlTextWriter tw;

        private StringWriter sw;

        private StringBuilder sb;

        private HttpWriter output;
        #endregion


        //#region  //压缩HMTL
        //private static string Compress(string text)
        //{
        //    Regex reg = new Regex(@"\s*(</?[^\s/>]+[^>]*>)\s+(</?[^\s/>]+[^>]*>)\s*");
        //    text = reg.Replace(text,m=>m.Groups[1].Value+m.Groups[2].Value);

        //    reg = new Regex(@"(?<=>)\s|\n|\t(?=<)");
        //    text = reg.Replace(text,string.Empty);
        //    return text;
        //}

        ///// <summary>  
        ///// 在执行Action的时候，就把需要的Writer存起来  
        ///// </summary>  
        ///// <param name="filterContext">上下文</param>  
        //protected override void OnActionExecuting(ActionExecutingContext filterContext)
        //{
        //    sb = new StringBuilder();
        //    sw = new StringWriter(sb);
        //    tw = new HtmlTextWriter(sw);
        //    output = (HttpWriter)filterContext.RequestContext.HttpContext.Response.Output;
        //    filterContext.RequestContext.HttpContext.Response.Output = tw;

        //    base.OnActionExecuting(filterContext);
        //}

        ///// <summary>  
        ///// 在执行完成后，处理得到的HTML，并将他输出到前台  
        ///// </summary>  
        ///// <param name="filterContext"></param>  
        //protected override void OnResultExecuted(ResultExecutedContext filterContext)
        //{
        //    string response = Compress(sb.ToString());

        //    output.Write(response);
        //}
        //#endregion

    }
    

}