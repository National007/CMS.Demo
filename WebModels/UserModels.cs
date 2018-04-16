using HH.Tools.ExcelFactory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebModels
{
   public class UserModels:Entity
    {
        [ExcelMap("账号")]
        public string Account { get; set; }
        [ExcelMap("密码")]
        public string Password { get; set; }
        [ExcelMap("姓名")]
        public string Name { get; set; }
        [ExcelMap("性别")]
        public int Sex { get; set; }
        [ExcelMap("状态")]
        public int Status { get; set; }
        [ExcelMap("编码")]
        public string BizCode { get; set; }
        [ExcelMap("创建时间")]
        public DateTime CreateTime { get; set; }
        [ExcelMap("类别")]
        public string CrateId { get; set; }
        [ExcelMap("类型")]
        public string TypeName { get; set; }
        [ExcelMap("类型ID")]
        public string TypeId { get; set; }

        #region extend
        [ExcelMap("姓名")]
        public string SexName { get; set; }
        #endregion
    }
}
