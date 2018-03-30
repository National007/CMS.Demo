/* ==============================
*
* Author: 漆勇
* Time：2016-1-20 13:32:29
* FileName：ExcelResultModel
* Version：V1.0.1
* ===============================
*/
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Application.ExcelHelper
{
    public class ExcelResultModel
    {
        public ExcelResultModel()
        {
            this.Success = true;
            this.ErrorList = new List<string>();
        }
     
        public bool Success { get; set; }
        public int Row { get; set; }
        public int Cell { get; set; }
        public string ErrorMessage { get; set;  }
        public List<String> ErrorList { get; set; }
        public IEnumerable List { get; set; }
    }
}
