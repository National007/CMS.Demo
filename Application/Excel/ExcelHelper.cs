using org.in2bits.MyXls;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;

namespace Application.Excel
{
    /// <summary>
    /// 导出excel
    /// 参考链接：http://code1.okbase.net/codefile/XlsDocument.cs_2013123125444_67.htm
    /// </summary>
    public class ExcelHelper
    {
        /// <summary>
        /// 无须保存到服务器导出Excel，返回byte[]
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="title">标题</param>
        /// <param name="list">数据集合</param>
        /// <param name="header">表头</param>
        /// <param name="items">栏目集合</param>
        /// <returns></returns>
        public static byte[] Export<T>(string title, List<T> list, string header = "", List<string> items = null) where T : new()
        {
            var buffer = new byte[1];

            try
            {
                string excelPath = string.Empty;

                var properties = typeof(T).GetProperties();

                #region 提取表头信息
                if (items == null || items.Count <= 0)
                {
                    items = new List<string>();

                    foreach (var propty in properties)
                    {
                        // 字段忽略excel生成
                        var ignoreAttr = (ExcelIgnoreAttribute)Attribute.GetCustomAttribute(propty, typeof(ExcelIgnoreAttribute));
                        if (ignoreAttr != null)
                        {
                            continue;
                        }
                        // 获取display特性
                        var displayAttr = (DisplayAttribute)Attribute.GetCustomAttribute(propty, typeof(DisplayAttribute));
                        if (displayAttr != null)
                        {
                            items.Add(displayAttr.Name);
                        }
                    }
                }
                #endregion

                int num = list.Count;//数据总条数
                int ColumnIndexEnd = 30;//默认是30列  
                if (items.Count > 0)
                {
                    ColumnIndexEnd = items.Count; //设置列=表头数
                }
                string WorksheetsName = title; //设置工作簿名称

                // 创建空的xls文档
                XlsDocument xls = new XlsDocument();
                // 创建一个工作页
                Worksheet sheet = xls.Workbook.Worksheets.Add(WorksheetsName);
                // 设置文档列属性
                ColumnInfo colInfo = new ColumnInfo(xls, sheet);
                colInfo.ColumnIndexStart = 0;//列开始
                colInfo.ColumnIndexEnd = (ushort)ColumnIndexEnd;//列结束
                colInfo.Collapsed = true;//合并边框
                colInfo.Width = 3200;
                sheet.AddColumnInfo(colInfo);

                Cells cells = sheet.Cells;

                #region 设置excel数据列标题格式

                XF xfDataHead = xls.NewXF();
                xfDataHead.HorizontalAlignment = HorizontalAlignments.Centered;
                xfDataHead.VerticalAlignment = VerticalAlignments.Centered;
                xfDataHead.Font.FontName = "宋体";
                xfDataHead.Font.Bold = true;
                xfDataHead.UseBorder = true;
                xfDataHead.BottomLineStyle = 1;
                xfDataHead.BottomLineColor = Colors.Black;
                xfDataHead.TopLineStyle = 1;
                xfDataHead.TopLineColor = Colors.Black;
                xfDataHead.LeftLineStyle = 1;
                xfDataHead.LeftLineColor = Colors.Black;
                xfDataHead.RightLineStyle = 1;
                xfDataHead.RightLineColor = Colors.Black;

                #endregion 设置excel数据列标题格式

                #region 写入Excel标题

                xfDataHead.Font.Height = 20 * 20;
                MergeArea ma = new MergeArea(1, 2, 1, ColumnIndexEnd);
                sheet.AddMergeArea(ma);
                cells.Add(1, 1, title, xfDataHead);

                #endregion

                #region 写入excel统计内容
                var hasOther = !string.IsNullOrEmpty(header);
                if (hasOther)
                {
                    XF print = xls.NewXF();
                    print.HorizontalAlignment = HorizontalAlignments.Left;
                    print.VerticalAlignment = VerticalAlignments.Centered;
                    print.Font.FontName = "宋体";
                    print.Font.Bold = false;
                    print.UseBorder = false;
                    print.Font.Height = 20 * 9;

                    MergeArea maA = new MergeArea(3, 4, 1, ColumnIndexEnd);
                    sheet.AddMergeArea(maA);
                    cells.Add(3, 1, header, print);
                }

                #endregion

                #region 写入Excel添加表头

                xfDataHead.Font.Height = 20 * 10;
                for (int i = 0; i < items.Count; i++)
                {
                    cells.Add(3 + Convert.ToInt32(hasOther) * 2, i + 1, items[i], xfDataHead);
                }

                #endregion

                #region 设置Excel数据格式

                XF xf = xls.NewXF();
                xf.HorizontalAlignment = HorizontalAlignments.Left;
                xf.VerticalAlignment = VerticalAlignments.Centered;
                xf.UseBorder = true;
                xf.TopLineStyle = 1;
                xf.TopLineColor = Colors.Black;
                xf.BottomLineStyle = 1;
                xf.BottomLineColor = Colors.Black;
                xf.LeftLineStyle = 1;
                xf.LeftLineColor = Colors.Black;
                xf.RightLineStyle = 1;
                xf.RightLineColor = Colors.Black;
                xf.Font.Height = 20 * 10;

                #endregion

                #region 写入excel数据

                for (int i = 0; i < list.Count; i++)
                {
                    var propertyInfo = typeof(T).GetProperties();
                    // 忽略次数
                    var ignoreTimes = 0;
                    for (int j = 0; j < propertyInfo.Count(); j++)
                    {
                        // 字段忽略excel生成
                        var attrs = propertyInfo[j].GetCustomAttributes(typeof(ExcelIgnoreAttribute), false);
                        if (attrs.Length > 0)
                        {
                            ignoreTimes++;
                            continue;
                        }
                        // 获取属性的值
                        var value = propertyInfo[j].GetValue(list[i], null);
                        // 写入属性值到单元格中
                        cells.Add(i + 4 + Convert.ToInt32(hasOther) * 2, j + 1 - ignoreTimes, value, xf);
                    }
                }

                #endregion

                using (var stream = new MemoryStream())
                {
                    xls.Save(stream);

                    buffer = stream.GetBuffer();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return buffer;
        }

        /// <summary>
        /// 导出Excel,返回地址(保存文件)
        /// </summary>
        /// <typeparam name="T">泛型参数</typeparam>
        /// <param name="list">生成excel的集合数据</param>
        /// <param name="excelName">excel的页名称</param>
        /// <param name="header">excel的表头集合</param>
        /// <param name="savePath">excel的保存路径</param>
        /// <param name="printInfo">统计信息</param>
        /// <returns></returns>
        public static string ConvertToExcel<T>(List<T> list, string excelName, string savePath, string printInfo = "", List<string> header = null) where T : new()
        {
            string excelPath = string.Empty;

            var properties = typeof(T).GetProperties();

            #region 提取表头信息
            if (header == null || header.Count <= 0)
            {
                header = new List<string>();

                foreach (var propty in properties)
                {
                    // 字段忽略excel生成
                    var ignoreAttr = (ExcelIgnoreAttribute)Attribute.GetCustomAttribute(propty, typeof(ExcelIgnoreAttribute));
                    if (ignoreAttr != null)
                    {
                        continue;
                    }
                    // 获取display特性
                    var displayAttr = (DisplayAttribute)Attribute.GetCustomAttribute(propty, typeof(DisplayAttribute));
                    if (displayAttr != null)
                    {
                        header.Add(displayAttr.Name);
                    }
                }
            }
            #endregion

            int num = list.Count;//数据总条数
            int ColumnIndexEnd = 30;//默认是30列  
            if (header.Count > 0)
            {
                ColumnIndexEnd = header.Count; //设置列=表头数
            }
            string WorksheetsName = excelName; //设置工作簿名称
            if (string.IsNullOrEmpty(WorksheetsName))
            {
                WorksheetsName = "第一页";
            }

            try
            {
                // 创建空的xls文档
                XlsDocument xls = new XlsDocument();
                // 创建一个工作页
                Worksheet sheet = xls.Workbook.Worksheets.Add(WorksheetsName);
                // 设置文档列属性
                ColumnInfo colInfo = new ColumnInfo(xls, sheet);
                colInfo.ColumnIndexStart = 0;//列开始
                colInfo.ColumnIndexEnd = (ushort)ColumnIndexEnd;//列结束
                colInfo.Collapsed = true;//合并边框
                colInfo.Width = 3200;
                sheet.AddColumnInfo(colInfo);

                Cells cells = sheet.Cells;

                #region 设置excel数据列标题格式

                XF xfDataHead = xls.NewXF();
                xfDataHead.HorizontalAlignment = HorizontalAlignments.Centered;
                xfDataHead.VerticalAlignment = VerticalAlignments.Centered;
                xfDataHead.Font.FontName = "宋体";
                xfDataHead.Font.Bold = true;
                xfDataHead.UseBorder = true;
                xfDataHead.BottomLineStyle = 1;
                xfDataHead.BottomLineColor = Colors.Black;
                xfDataHead.TopLineStyle = 1;
                xfDataHead.TopLineColor = Colors.Black;
                xfDataHead.LeftLineStyle = 1;
                xfDataHead.LeftLineColor = Colors.Black;
                xfDataHead.RightLineStyle = 1;
                xfDataHead.RightLineColor = Colors.Black;

                #endregion 设置excel数据列标题格式

                #region 写入Excel标题

                xfDataHead.Font.Height = 20 * 20;
                MergeArea ma = new MergeArea(1, 2, 1, ColumnIndexEnd);
                sheet.AddMergeArea(ma);
                cells.Add(1, 1, excelName, xfDataHead);

                #endregion

                #region 写入excel统计内容
                var hasOther = !string.IsNullOrEmpty(printInfo);
                if (hasOther)
                {
                    XF print = xls.NewXF();
                    print.HorizontalAlignment = HorizontalAlignments.Left;
                    print.VerticalAlignment = VerticalAlignments.Centered;
                    print.Font.FontName = "宋体";
                    print.Font.Bold = false;
                    print.UseBorder = false;
                    print.Font.Height = 20 * 9;

                    MergeArea maA = new MergeArea(3, 4, 1, ColumnIndexEnd);
                    sheet.AddMergeArea(maA);
                    cells.Add(3, 1, printInfo, print);
                }

                #endregion

                #region 写入Excel添加表头

                xfDataHead.Font.Height = 20 * 10;
                for (int i = 0; i < header.Count; i++)
                {
                    cells.Add(3 + Convert.ToInt32(hasOther) * 2, i + 1, header[i], xfDataHead);
                }

                #endregion

                #region 设置Excel数据格式

                XF xf = xls.NewXF();
                xf.HorizontalAlignment = HorizontalAlignments.Left;
                xf.VerticalAlignment = VerticalAlignments.Centered;
                xf.UseBorder = true;
                xf.TopLineStyle = 1;
                xf.TopLineColor = Colors.Black;
                xf.BottomLineStyle = 1;
                xf.BottomLineColor = Colors.Black;
                xf.LeftLineStyle = 1;
                xf.LeftLineColor = Colors.Black;
                xf.RightLineStyle = 1;
                xf.RightLineColor = Colors.Black;
                xf.Font.Height = 20 * 10;

                #endregion

                #region 写入excel数据

                for (int i = 0; i < list.Count; i++)
                {
                    var propertyInfo = typeof(T).GetProperties();
                    // 忽略次数
                    var ignoreTimes = 0;
                    for (int j = 0; j < propertyInfo.Count(); j++)
                    {
                        // 字段忽略excel生成
                        var attrs = propertyInfo[j].GetCustomAttributes(typeof(ExcelIgnoreAttribute), false);
                        if (attrs.Length > 0)
                        {
                            ignoreTimes++;
                            continue;
                        }
                        // 获取属性的值
                        var value = propertyInfo[j].GetValue(list[i], null);
                        // 写入属性值到单元格中
                        cells.Add(i + 4 + Convert.ToInt32(hasOther) * 2, j + 1 - ignoreTimes, value, xf);
                    }
                }

                #endregion

                #region 保存excel文件
                string fileName = DateTime.Now.ToString("yyyyMMddHHmmss");
                xls.FileName = fileName;

                var rootPath = GetRootPath();
                var newPath = rootPath + savePath;
                if (!System.IO.Directory.Exists(newPath))//判断是否存在着文件夹
                {
                    System.IO.Directory.CreateDirectory(newPath);//不存在就创建目录
                }

                xls.Save(newPath);
                excelPath = savePath + "//" + fileName + ".xls";
                #endregion
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
            return excelPath;
        }

        /// <summary>
        /// 获取程序根目录
        /// </summary>
        /// <returns></returns>
        public static string GetRootPath()
        {
            string AppPath = "";
            HttpContext HttpCurrent = HttpContext.Current;
            if (HttpCurrent != null)
            {
                AppPath = HttpCurrent.Server.MapPath("~");
            }
            else
            {
                AppPath = AppDomain.CurrentDomain.BaseDirectory;
                if (Regex.Match(AppPath, @"\\$", RegexOptions.Compiled).Success)
                    AppPath = AppPath.Substring(0, AppPath.Length - 1);
            }
            return AppPath;
        }
    }
}
