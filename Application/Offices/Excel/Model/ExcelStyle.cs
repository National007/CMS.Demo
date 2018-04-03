using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

///用于设置导出样式的实体类

namespace Application.Offices.Excel.Model
{
    public class ExcelStyle
    {
        /// <summary>
        /// 要求改变底色的列数，默认空
        /// </summary>
        public List<int> listObj { get; set; }
        /// <summary>
        /// 需要转百分比样式的列（列名集合）
        /// </summary>
        public List<string> PercentFields { get; set; }
        /// <summary>
        /// 是否转换样式
        /// </summary>
        public bool IsConvertType { get; set; }

        /// <summary>
        /// 需要行变色的关键字
        /// </summary>
        public string word { get; set; }
        /// <summary>
        /// 列颜色设置，默认空
        /// </summary>
        public short ColColour { get; set; }

        /// <summary>
        /// 行颜色设置，默认空
        /// </summary>
        public short RowColour { get; set; }

    }
}
