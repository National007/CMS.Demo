using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Application.Offices.Excel.Model
{
    public class TemplateMode
    {
        /// <summary>
        /// 行号
        /// </summary>
        public int row { get; set; }
        /// <summary>
        /// 列号
        /// </summary>
        public int cell { get; set; }
        /// <summary>
        /// 数据值
        /// </summary>
        public string value { get; set; }
        /// <summary>
        /// 字段类型
        /// </summary>
        public string FieldType { get; set; }
        /// <summary>
        /// 字段名
        /// </summary>
        public string FieldName { get; set; }
        /// <summary>
        /// 背景色
        /// </summary>
        public short BackgroundColor { get; set; }
    }
}
