using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.ExcelHelper
{
    public interface IExcelService
    {
        /// <summary>
        /// Excel表格导入
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="file"></param>
        /// <param name="extend">文件扩展名</param>
        /// <param name="isContainNull">是否包含Null值</param>
        /// <returns></returns>
        ExcelResultModel LoadExcel<T>(Stream file, string extend, bool isContainNull = false);
        /// <summary>
        /// Excel表格导入
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="bytes"></param>
        /// <param name="extend"></param>
        /// <param name="isContainNull"></param>
        /// <returns></returns>
        ExcelResultModel LoadExcel<T>(Byte[] bytes, string extend, bool isContainNull);

        /// <summary>
        /// 导出Excel
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list">数据集合</param>
        /// <param name="version">Excel版本号 默认为07版</param>
        /// <returns></returns>
        string ExportExcel<T>(IEnumerable<T> list, ExcelVersion version = ExcelVersion.XSSF);
        /// <summary>
        /// 导出Excel
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list">数据集合</param>
        /// <param name="fields">字段集合</param>
        /// <param name="version">Excel版本号 默认为07版</param>
        /// <returns></returns>
        string ExportExcel<T>(IEnumerable<T> list, string[] fields, ExcelVersion version = ExcelVersion.XSSF);
        
    }
}
