using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CMS.MVC.Model
{
    public class ReportUserModel
    {
        [Display(Name = "账号")]
        public string Account { get; set; }
        [Display(Name = "密码")]
        public string Password { get; set; }
        [Display(Name = "姓名")]
        public string Name { get; set; }
        [Display(Name = "性别")]
        public string SexName { get; set; }
       
    }
}