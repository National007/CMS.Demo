
using Application;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CMS.MVC.Controllers
{
    public class LoginController : Controller
    {
        // GET: Login
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 获取验证码
        /// </summary>
        /// <returns></returns>
        public FileResult GetValidateImg()
        {
            //首先实例化验证码的类
            ValidateCode validateCode = new ValidateCode();
            //生成验证码指定的长度
            string code = validateCode.GetRandomString(4);
            //验证码存入Session
            Session["validateCode"] = code;
            //创建验证码的图片
            byte[] bytes = validateCode.CreateImage(code);
            //最后将验证码返回  
            return File(bytes, @"image/jpeg");
        }
    }
}