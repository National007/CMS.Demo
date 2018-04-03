using KS.Util.Offices.Model;
using NPOI.HPSF;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Configuration;
using NPOI.DDF;
using NPOI.HSSF.Record;
using System.ComponentModel;
using Application.Offices.Excel.Model;

namespace KS.Util.Offices
{
    public class ExcelHelper
    {
        #region Excel导出方法 ExcelDownload

        /// <summary>
        /// Excel导出下载
        /// </summary>
        /// <param name="dtSource">DataTable数据源</param>
        /// <param name="excelConfig">导出设置包含文件名、标题、列设置</param>
        /// <param name="isRemoveColumns">是否删除列</param>
        public static void ExcelDownload(DataTable dtSource, ExcelConfig excelConfig, bool isRemoveColumns = false)
        {
            HttpContext curContext = HttpContext.Current;
            // 设置编码和附件格式
            curContext.Response.ContentType = "application/ms-excel";
            curContext.Response.ContentEncoding = Encoding.UTF8;
            curContext.Response.Charset = "";
            curContext.Response.AppendHeader("Content-Disposition",
                "attachment;filename=" + excelConfig.FileName);//HttpUtility.UrlEncode(excelConfig.FileName, Encoding.UTF8));
            //调用导出具体方法Export()
            curContext.Response.BinaryWrite(ExportMemoryStream(dtSource, excelConfig, isRemoveColumns).GetBuffer());
            curContext.Response.End();
        }


        /// <summary>
        /// Excel导出下载
        /// </summary>
        /// <param name="list">数据源</param>
        /// <param name="templdateName">模板文件名</param>
        /// <param name="newFileName">文件名</param>
        public static void ExcelDownload(List<TemplateMode> list, string templdateName, string newFileName, ExcelStyle excelStyle = null)
        {
            HttpResponse response = System.Web.HttpContext.Current.Response;
            response.Clear();
            response.Charset = "UTF-8";
            response.ContentType = "application/vnd-excel";//"application/vnd.ms-excel";
            System.Web.HttpContext.Current.Response.AddHeader("Content-Disposition", string.Format("attachment; filename=" + newFileName));
            System.Web.HttpContext.Current.Response.BinaryWrite(ExportListByTempale(list, templdateName, excelStyle).ToArray());
        }



        /// <summary>
        /// Excel导出下载
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="newFileName"></param>
        public static void ExcelDownload(byte[] buffer, string newFileName)
        {
            HttpResponse response = System.Web.HttpContext.Current.Response;
            response.Clear();
            response.Charset = "UTF-8";
            response.ContentType = "application/vnd-excel";//"application/vnd.ms-excel";
            System.Web.HttpContext.Current.Response.AddHeader("Content-Disposition", string.Format("attachment; filename=" + newFileName));
            System.Web.HttpContext.Current.Response.BinaryWrite(buffer);
        }
        #endregion

        #region DataTable导出到Excel文件excelConfig中FileName设置为全路径
        /// <summary>
        /// DataTable导出到Excel文件 Export()
        /// </summary>
        /// <param name="dtSource">DataTable数据源</param>
        /// <param name="excelConfig">导出设置包含文件名、标题、列设置</param>
        public static void ExcelExport(DataTable dtSource, ExcelConfig excelConfig)
        {
            using (MemoryStream ms = ExportMemoryStream(dtSource, excelConfig))
            {
                using (FileStream fs = new FileStream(excelConfig.FileName, FileMode.Create, FileAccess.Write))
                {
                    byte[] data = ms.ToArray();
                    fs.Write(data, 0, data.Length);
                    fs.Flush();
                }
            }
        }

        /// <summary>
        /// 封装带样式导出方法
        /// </summary>
        /// <typeparam name="T">传入数据的实体类型</typeparam>
        /// <param name="list">传入数据集合</param>
        /// <param name="excels">样式类</param>
        /// <param name="fileTitle">列头字典集合（可空）</param>
        /// <returns></returns>
        public static HSSFWorkbook OutputSearchResultAll<T>(List<T> list, ExcelStyle excels, Dictionary<string, string> scoureType = null)
        {
            List<int> Col = excels.listObj;

            string word = excels.word;
            short colColour = 13;
            short rowColour = 49;
            if (excels.ColColour != 0)
            {
                colColour = excels.ColColour;
            }
            if (excels.RowColour != 0) { rowColour = excels.RowColour; }

            HSSFWorkbook hssfWorkbook = new HSSFWorkbook();
            ISheet sheet = hssfWorkbook.CreateSheet("Sheet1");
            IDataFormat format = hssfWorkbook.CreateDataFormat();
            //format = HSSFDataFormat.GetBuiltinFormat("General");
            //手填单元格样式
            HSSFCellStyle sheetStyle = (HSSFCellStyle)hssfWorkbook.CreateCellStyle();

            sheetStyle.FillPattern = NPOI.SS.UserModel.FillPattern.SolidForeground;
            sheetStyle.FillForegroundColor = colColour;
            sheetStyle.Alignment = HorizontalAlignment.Center;
            //合计行单元格样式
            HSSFCellStyle sheetStyleOne = (HSSFCellStyle)hssfWorkbook.CreateCellStyle();
            sheetStyleOne.FillPattern = NPOI.SS.UserModel.FillPattern.SolidForeground;
            sheetStyleOne.FillForegroundColor = rowColour;
            sheetStyleOne.Alignment = HorizontalAlignment.Center;
            sheetStyle.BorderBottom = NPOI.SS.UserModel.BorderStyle.Thin;
            sheetStyle.BorderLeft = NPOI.SS.UserModel.BorderStyle.Thin;
            sheetStyle.BorderRight = NPOI.SS.UserModel.BorderStyle.Thin;
            sheetStyle.BorderTop = NPOI.SS.UserModel.BorderStyle.Thin;

            sheetStyle.TopBorderColor = NPOI.HSSF.Util.HSSFColor.Aqua.Index;
            sheetStyle.LeftBorderColor = NPOI.HSSF.Util.HSSFColor.Aqua.Index;
            sheetStyle.BottomBorderColor = NPOI.HSSF.Util.HSSFColor.Aqua.Index;
            sheetStyle.RightBorderColor = NPOI.HSSF.Util.HSSFColor.Aqua.Index;
            //手填——百分比样式
            HSSFCellStyle pSheetStyle = (HSSFCellStyle)hssfWorkbook.CreateCellStyle();
            pSheetStyle.CloneStyleFrom(sheetStyle);
            pSheetStyle.DataFormat = format.GetFormat("0.0%");
            //合计——百分比样式
            HSSFCellStyle pSheetStyleOne = (HSSFCellStyle)hssfWorkbook.CreateCellStyle();
            pSheetStyleOne.CloneStyleFrom(sheetStyleOne);
            pSheetStyleOne.DataFormat = format.GetFormat("0.0%");
            //普通——百分比样式
            HSSFCellStyle pSheetStyleNone = (HSSFCellStyle)hssfWorkbook.CreateCellStyle();
            pSheetStyleNone.DataFormat = format.GetFormat("0.0%");
            pSheetStyleNone.Alignment = HorizontalAlignment.Center;
            //普通——
            HSSFCellStyle styleNone = (HSSFCellStyle)hssfWorkbook.CreateCellStyle();
            styleNone.Alignment = HorizontalAlignment.Center;
            sheet.DefaultColumnWidth = 12;
            var t = typeof(T);

            //设置列头
            if (scoureType == null)
            {
                //默认列头
                SetDefaultHeader<T>(Col, sheet, sheetStyle);
            }
            else
            {
                //根据传入列头数据设置列头
                SetHeader(Col, sheet, sheetStyle, scoureType);
            }
            //如果数据为空直接返回列头
            if (list.Count == 0)
                return hssfWorkbook;
            //获取数据类字段集合
            System.Reflection.PropertyInfo[] properties = list[0].GetType().GetProperties(System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public);
            int k = 0;
            foreach (var ok in list)
            {
                IRow dataRow = sheet.CreateRow(k + 2);
                List<object> listObj = new List<object>();
                int f = 0;
                foreach (var item in properties)
                {
                    string name = item.Name;
                    object value = item.GetValue(ok, null);
                    if (value == null)
                    {
                        dataRow.CreateCell(f, CellType.Blank);
                    }
                    else if (value is string)
                    { dataRow.CreateCell(f, CellType.String).SetCellValue(value.ToString()); }
                    else if (value is int)
                    {
                        dataRow.CreateCell(f).SetCellValue(Convert.ToInt32(value));
                    }
                    else if (value is decimal)
                    {
                        dataRow.CreateCell(f, CellType.Numeric).SetCellValue(Convert.ToDouble(value));
                    }
                    //普通单元格样式
                    dataRow.GetCell(f).CellStyle = styleNone;
                    //普通单元格百分比列
                    if ((properties[f].Name.Contains("Pro") || properties[f].Name.Contains("PercentBand")) && !properties[f].Name.Contains("ProfitRate"))
                    {
                        dataRow.GetCell(f).CellStyle = pSheetStyleNone;
                    }
                    //手填部分变色
                    if (Col != null && Col.Contains(f))
                    {
                        if ((properties[f].Name.Contains("Pro") || properties[f].Name.Contains("PercentBand")) && !properties[f].Name.Contains("ProfitRate"))
                            dataRow.GetCell(f).CellStyle = pSheetStyle;
                        else
                            dataRow.GetCell(f).CellStyle = sheetStyle;
                    }
                    listObj.Add(value);
                    f++;
                }
                //合计变色(如果存在合计行变色设置，且当前行数据中有合计标识值，则该行变色)
                if (word != null && listObj.Contains(word))
                {
                    for (int G = 0; G < properties.Length; G++)
                    {
                        if ((properties[G].Name.Contains("Pro") || properties[G].Name.Contains("PercentBand")) && !properties[G].Name.Contains("ProfitRate"))
                            dataRow.GetCell(G).CellStyle = pSheetStyleOne;
                        else
                            dataRow.GetCell(G).CellStyle = sheetStyleOne;
                    }
                }
                k++;
            }
            return hssfWorkbook;
        }

         
        private static void SetHeader(List<int> Col, ISheet sheet, HSSFCellStyle sheetStyle, Dictionary<string, string> headerDict)
        {
            //创建Excel首行
            IRow rowHeader = sheet.CreateRow(0);    //英文字段行
            IRow rowHeader2 = sheet.CreateRow(1);   //中文字段行

            //循环字段集合，并将字段名插入列头
            for (int i = 0; i < headerDict.Count; i++)
            {
                var node = headerDict.ElementAt(i);
                rowHeader.CreateCell(i, CellType.String).SetCellValue(node.Key);
                rowHeader2.CreateCell(i, CellType.String).SetCellValue(node.Value);
            }
            //对变色列进行变色
            if (Col != null)
            {
                foreach (var Icol in Col)
                {
                    if (
                    Icol > headerDict.Count || Icol < 0
                    )
                    {
                        throw new Exception("列数超过实体类个数或列数为负数");
                    }
                    rowHeader.GetCell(Icol).CellStyle = sheetStyle;
                    rowHeader2.GetCell(Icol).CellStyle = sheetStyle;
                }
            }
        }

        private static void SetDefaultHeader<T>(List<int> Col, ISheet sheet, HSSFCellStyle sheetStyle)
        {
            var dataType = typeof(T);
            var properties = dataType.GetProperties(System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public);
            //创建Excel首行
            IRow rowHeader = sheet.CreateRow(0);
            int i = 0;
            //循环字段集合，并将字段名插入列头
            foreach (var item in properties)
            {
                string name = item.Name;
                rowHeader.CreateCell(i, CellType.String).SetCellValue(name);
                i++;
            }
            //对变色列进行变色
            if (Col != null)
            {
                foreach (var Icol in Col)
                {
                    if (
                    Icol > i || Icol < 0
                    )
                    {
                        throw new Exception("列数超过实体类个数或列数为负数");
                    }
                    rowHeader.GetCell(Icol).CellStyle = sheetStyle;
                }
            }
            IRow rowHeader2 = sheet.CreateRow(1);
            int j = 0;
            //插入字段中文注释，到Excel第二行
            foreach (var item in properties)
            {
                string name = item.Name;
                var pName = dataType.GetProperty(name);
                object[] displayNames = pName.GetCustomAttributes(true);

                var displayName = displayNames[0];
                if (displayName.ToString() != "System.ComponentModel.DisplayNameAttribute")
                {
                    displayName = displayNames[1];
                }
                System.Reflection.PropertyInfo[] properties2 = displayName.GetType().GetProperties(System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public);

                if (displayName == null)
                {
                    throw new Exception("你的实体类没有注明displaName");
                }
                var ChileName = properties2[0].GetValue(displayName, null).ToString();
                rowHeader2.CreateCell(j, CellType.String).SetCellValue(ChileName);
                j++;
            }
            if (Col != null)
            {
                foreach (var Icol in Col)
                {
                    rowHeader2.GetCell(Icol).CellStyle = sheetStyle;
                }
            }
        }
        #endregion


        private static readonly string SharedImagePath = ConfigurationManager.AppSettings["sharedImagePath"];//共享盘图片地址配置



        #region DataTable导出到Excel的MemoryStream

        /// <summary>
        /// DataTable导出到Excel的MemoryStream Export()
        /// </summary>
        /// <param name="dtSource">DataTable数据源</param>
        /// <param name="excelConfig">导出设置包含文件名、标题、列设置</param>
        /// <param name="isRemoveColumns"></param>
        public static MemoryStream ExportMemoryStream(DataTable dtSource, ExcelConfig excelConfig, bool isRemoveColumns = false)
        {
            if (isRemoveColumns)
            {
                int colint = 0;
                for (int i = 0; i < dtSource.Columns.Count; )
                {
                    DataColumn column = dtSource.Columns[i];
                    //if (colint >= excelConfig.ColumnEntity.Count || excelConfig.ColumnEntity[colint].Column != column.ColumnName)
                    if (colint >= excelConfig.ColumnEntity.Count || excelConfig.ColumnEntity.Where(s => s.Column == column.ColumnName).Count() == 0)
                    {
                        dtSource.Columns.Remove(column.ColumnName);
                    }
                    else
                    {
                        ColumnEntity columnentity = excelConfig.ColumnEntity.Find(t => t.Column == dtSource.Columns[i].ColumnName);
                        dtSource.Columns[i].ColumnName = columnentity.ExcelColumn;//修改列头名
                        i++;
                        colint++;
                    }

                }
            }


            HSSFWorkbook workbook = new HSSFWorkbook();
            ISheet sheet = workbook.CreateSheet();

            #region 右击文件 属性信息
            {
                DocumentSummaryInformation dsi = PropertySetFactory.CreateDocumentSummaryInformation();
                dsi.Company = "NPOI";
                workbook.DocumentSummaryInformation = dsi;

                SummaryInformation si = PropertySetFactory.CreateSummaryInformation();
                si.Author = "曾丁丁"; //填加xls文件作者信息
                si.ApplicationName = "凯盛服饰"; //填加xls文件创建程序信息
                si.LastAuthor = "曾丁丁"; //填加xls文件最后保存者信息
                si.Comments = "曾丁丁"; //填加xls文件作者信息
                si.Title = "标题信息"; //填加xls文件标题信息
                si.Subject = "主题信息";//填加文件主题信息
                si.CreateDateTime = System.DateTime.Now;
                workbook.SummaryInformation = si;
            }
            #endregion

            #region 设置标题样式
            ICellStyle headStyle = workbook.CreateCellStyle();
            int[] arrColWidth = new int[dtSource.Columns.Count];
            string[] arrColName = new string[dtSource.Columns.Count];//列名
            ICellStyle[] arryColumStyle = new ICellStyle[dtSource.Columns.Count];//样式表
            headStyle.Alignment = HorizontalAlignment.Center; // ------------------
            if (excelConfig.Background != new Color())
            {
                if (excelConfig.Background != new Color())
                {
                    headStyle.FillPattern = FillPattern.SolidForeground;
                    headStyle.FillForegroundColor = GetXLColour(workbook, excelConfig.Background);
                }
            }
            IFont font = workbook.CreateFont();
            font.FontHeightInPoints = excelConfig.TitlePoint;
            if (excelConfig.ForeColor != new Color())
            {
                font.Color = GetXLColour(workbook, excelConfig.ForeColor);
            }
            font.Boldweight = 700;
            headStyle.SetFont(font);
            #endregion

            #region 列头及样式
            ICellStyle cHeadStyle = workbook.CreateCellStyle();
            cHeadStyle.Alignment = HorizontalAlignment.Center; // ------------------
            IFont cfont = workbook.CreateFont();
            cfont.FontHeightInPoints = excelConfig.HeadPoint;
            cHeadStyle.SetFont(cfont);
            #endregion

            #region 设置内容单元格样式
            foreach (DataColumn item in dtSource.Columns)
            {
                ICellStyle columnStyle = workbook.CreateCellStyle();
                columnStyle.Alignment = HorizontalAlignment.Center;
                arrColWidth[item.Ordinal] = Encoding.GetEncoding(936).GetBytes(item.ColumnName.ToString()).Length;
                arrColName[item.Ordinal] = item.ColumnName.ToString();
                if (excelConfig.ColumnEntity != null)
                {
                    ColumnEntity columnentity = excelConfig.ColumnEntity.Find(t => t.Column == item.ColumnName);
                    if (columnentity != null)
                    {
                        arrColName[item.Ordinal] = columnentity.ExcelColumn;
                        if (columnentity.Width != 0)
                        {
                            arrColWidth[item.Ordinal] = columnentity.Width;
                        }
                        if (columnentity.Background != new Color())
                        {
                            if (columnentity.Background != new Color())
                            {
                                columnStyle.FillPattern = FillPattern.SolidForeground;
                                columnStyle.FillForegroundColor = GetXLColour(workbook, columnentity.Background);
                            }
                        }
                        if (columnentity.Font != null || columnentity.Point != 0 || columnentity.ForeColor != new Color())
                        {
                            IFont columnFont = workbook.CreateFont();
                            columnFont.FontHeightInPoints = 10;
                            if (columnentity.Font != null)
                            {
                                columnFont.FontName = columnentity.Font;
                            }
                            if (columnentity.Point != 0)
                            {
                                columnFont.FontHeightInPoints = columnentity.Point;
                            }
                            if (columnentity.ForeColor != new Color())
                            {
                                columnFont.Color = GetXLColour(workbook, columnentity.ForeColor);
                            }
                            columnStyle.SetFont(font);
                        }
                        columnStyle.Alignment = getAlignment(columnentity.Alignment);
                    }
                }
                arryColumStyle[item.Ordinal] = columnStyle;
            }
            if (excelConfig.IsAllSizeColumn)
            {
                #region 根据列中最长列的长度取得列宽
                for (int i = 0; i < dtSource.Rows.Count; i++)
                {
                    for (int j = 0; j < dtSource.Columns.Count; j++)
                    {
                        if (arrColWidth[j] != 0)
                        {
                            int intTemp = Encoding.GetEncoding(936).GetBytes(dtSource.Rows[i][j].ToString()).Length;
                            if (intTemp > arrColWidth[j])
                            {
                                arrColWidth[j] = intTemp;
                            }
                        }

                    }
                }
                #endregion
            }
            #endregion



            ICellStyle dateStyle = workbook.CreateCellStyle();
            IDataFormat format = workbook.CreateDataFormat();
            //不管你插入多少图片，都只要生成一个HSSFPatriarch 的对象,一定要放在循环外,只能声明一次,不然不能循环插入图片  
            HSSFPatriarch par = sheet.CreateDrawingPatriarch() as HSSFPatriarch;
            dateStyle.DataFormat = format.GetFormat("yyyy-mm-dd");
            int rowIndex = 0, colIndex = 0;
            foreach (DataRow row in dtSource.Rows)
            {
                colIndex = 0;
                #region 新建表，填充表头，填充列头，样式
                if (rowIndex == 65535 || rowIndex == 0)
                {
                    if (rowIndex != 0)
                    {
                        sheet = workbook.CreateSheet();
                    }

                    #region 表头及样式
                    {
                        if (excelConfig.Title != null)
                        {
                            IRow headerRow = sheet.CreateRow(0);
                            if (excelConfig.TitleHeight != 0)
                            {
                                headerRow.Height = (short)(excelConfig.TitleHeight * 20);
                            }
                            headerRow.HeightInPoints = 25;
                            headerRow.CreateCell(0).SetCellValue(excelConfig.Title);
                            headerRow.GetCell(0).CellStyle = headStyle;
                            sheet.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(0, 0, 0, dtSource.Columns.Count - 1)); // ------------------
                        }

                    }
                    #endregion

                    #region 列头及样式
                    {
                        IRow headerRow = sheet.CreateRow(1);
                        #region 如果设置了列标题就按列标题定义列头，没定义直接按字段名输出
                        foreach (DataColumn column in dtSource.Columns)
                        {
                            headerRow.CreateCell(column.Ordinal).SetCellValue(arrColName[column.Ordinal]);
                            headerRow.GetCell(column.Ordinal).CellStyle = cHeadStyle;
                            //设置列宽
                            sheet.SetColumnWidth(column.Ordinal, (arrColWidth[column.Ordinal] + 1) * 256);
                        }
                        #endregion
                    }
                    #endregion

                    rowIndex = 2;
                }
                #endregion

                #region 填充内容
                IRow dataRow = sheet.CreateRow(rowIndex);



                foreach (DataColumn column in dtSource.Columns)
                {

                    ICell newCell = dataRow.CreateCell(column.Ordinal);
                    newCell.CellStyle = arryColumStyle[column.Ordinal];
                    string drValue = row[column].ToString();
                    SetCell(newCell, dateStyle, column.DataType, drValue);
                    colIndex++;
                }
                #endregion
                rowIndex++;
            }
            using (MemoryStream ms = new MemoryStream())
            {
                workbook.Write(ms);
                ms.Flush();
                ms.Position = 0;
                return ms;
            }
        }
        #endregion

        #region ListExcel导出(加载模板)
        /// <summary>
        /// List根据模板导出ExcelMemoryStream
        /// </summary>
        /// <param name="list"></param>
        /// <param name="templdateName"></param>
        public static MemoryStream ExportListByTempale(List<TemplateMode> list, string templdateName, ExcelStyle excelStyle)
        {
            try
            {
                string templatePath = HttpContext.Current.Server.MapPath("/");
                string templdateName1 = string.Format("{0}{1}", templatePath, templdateName);

                FileStream fileStream = new FileStream(templdateName1, FileMode.Open, FileAccess.Read);
                ISheet sheet = null;
                if (templdateName.IndexOf(".xlsx", StringComparison.Ordinal) == -1)//2003
                {
                    IWorkbook hssfworkbook = new HSSFWorkbook(fileStream);
                    sheet = hssfworkbook.GetSheetAt(0);
                    SetPurchaseOrder(hssfworkbook, list, excelStyle);
                    sheet.ForceFormulaRecalculation = true;
                    using (MemoryStream ms = new MemoryStream())
                    {
                        hssfworkbook.Write(ms);
                        ms.Flush();
                        return ms;
                    }
                }
                else//2007
                {
                    IWorkbook xssfworkbook = new XSSFWorkbook(fileStream);
                    sheet = xssfworkbook.GetSheetAt(0);
                    SetPurchaseOrder(xssfworkbook, list, excelStyle);
                    sheet.ForceFormulaRecalculation = true;
                    using (MemoryStream ms = new MemoryStream())
                    {
                        xssfworkbook.Write(ms);
                        ms.Flush();
                        return ms;
                    }
                }

            }
            catch (Exception)
            {
                throw;
            }
        }
        /// <summary>
        /// 赋值单元格
        /// </summary>
        /// <param name="sheet"></param>
        /// <param name="list"></param>
        private static void SetPurchaseOrder(IWorkbook workbook, List<TemplateMode> list, ExcelStyle excelStyle)
        {
            var sheet = workbook.GetSheetAt(0);
            //百分比样式
            IDataFormat format = workbook.CreateDataFormat();
            XSSFCellStyle pSheetStyleNone = (XSSFCellStyle)workbook.CreateCellStyle();
            pSheetStyleNone.DataFormat = format.GetFormat("0.0%");
            pSheetStyleNone.Alignment = HorizontalAlignment.Center;

            try
            {

                foreach (var item in list)
                {
                    IRow row = null;
                    ICell cell = null;
                    row = sheet.GetRow(item.row);
                    if (row == null)
                    {
                        //在创建行时，给这个行赋值一个背景色样式
                        pSheetStyleNone.FillBackgroundColor = item.BackgroundColor;
                        row = sheet.CreateRow(item.row);

                    }
                    cell = row.GetCell(item.cell);
                    if (cell == null)
                    {
                        cell = row.CreateCell(item.cell);
                        cell.CellStyle = pSheetStyleNone;
                    }
                    if (item.value != null)
                    {
                        if (excelStyle != null && excelStyle.IsConvertType)
                        {
                            switch (item.FieldType.ToUpper())
                            {
                                case "SYSTEM.STRING":
                                    cell.SetCellValue(item.value);
                                    break;
                                case "SYSTEM.DATETIME":
                                    cell.SetCellValue(Convert.ToDateTime(item.value));
                                    break;
                                case "SYSTEM.INT32":
                                    cell.SetCellValue(Convert.ToInt32(item.value));
                                    break;
                                case "SYSTEM.DECIMAL":
                                    cell.SetCellValue(Convert.ToDouble(item.value));
                                    if (excelStyle.PercentFields.Any(s => s == item.FieldName))
                                        cell.CellStyle = pSheetStyleNone;
                                    break;
                                case "SYSTEM.NULLABLE`1[[SYSTEM.INT32, MSCORLIB, VERSION=4.0.0.0, CULTURE=NEUTRAL, PUBLICKEYTOKEN=B77A5C561934E089]]":
                                    cell.SetCellValue(Convert.ToInt32(item.value));
                                    break;
                                case "SYSTEM.NULLABLE`1[[SYSTEM.DECIMAL, MSCORLIB, VERSION=4.0.0.0, CULTURE=NEUTRAL, PUBLICKEYTOKEN=B77A5C561934E089]]":
                                    cell.SetCellValue(Convert.ToDouble(item.value));
                                    if (excelStyle.PercentFields.Any(s => s == item.FieldName))
                                        cell.CellStyle = pSheetStyleNone;
                                    break;
                                default:
                                    cell.SetCellValue(item.value.ToString());
                                    break;
                            }
                        }
                        else
                            cell.SetCellValue(item.value);
                    }
                    else
                    {
                        cell = row.CreateCell(item.cell, CellType.Blank);
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region 设置表格内容
        private static void SetCell(ICell newCell, ICellStyle dateStyle, Type dataType, string drValue)
        {
            switch (dataType.ToString())
            {
                case "System.String"://字符串类型
                    int intValue;
                    double doubleValue;
                    if (int.TryParse(drValue, out intValue))
                    {
                        newCell.SetCellValue(intValue);
                    }
                    else if (double.TryParse(drValue, out doubleValue))
                    {
                        newCell.SetCellValue(doubleValue);
                    }
                    else
                        newCell.SetCellValue(drValue);
                    break;
                case "System.DateTime"://日期类型
                    System.DateTime dateV;
                    if (System.DateTime.TryParse(drValue, out dateV))
                    {
                        newCell.SetCellValue(dateV);
                    }
                    else
                    {
                        newCell.SetCellValue("");
                    }
                    newCell.CellStyle = dateStyle;//格式化显示
                    break;
                case "System.Boolean"://布尔型
                    bool boolV = false;
                    bool.TryParse(drValue, out boolV);
                    newCell.SetCellValue(boolV);
                    break;
                case "System.Int16"://整型
                case "System.Int32":
                case "System.Int64":
                case "System.Byte":
                    int intV = 0;
                    int.TryParse(drValue, out intV);
                    newCell.SetCellValue(intV);
                    break;
                case "System.Decimal"://浮点型
                case "System.Double":
                    double doubV = 0;
                    double.TryParse(drValue, out doubV);
                    newCell.SetCellValue(doubV);
                    break;
                case "System.DBNull"://空值处理
                    newCell.SetCellValue("");
                    break;
                default:
                    newCell.SetCellValue("");
                    break;
            }
        }
        #endregion

        #region 从Excel导入
        /// <summary>
        /// 读取excel ,默认第一行为标头
        /// </summary>
        /// <param name="strFileName">excel文档路径</param>
        /// <returns></returns>
        public static DataTable ExcelImport(string strFileName)
        {
            DataTable dt = new DataTable();

            ISheet sheet = null;
            using (FileStream file = new FileStream(strFileName, FileMode.Open, FileAccess.Read))
            {
                if (strFileName.IndexOf(".xlsx", StringComparison.Ordinal) == -1)//2003
                {
                    HSSFWorkbook hssfworkbook = new HSSFWorkbook(file);
                    sheet = hssfworkbook.GetSheetAt(0);
                }
                else//2007
                {
                    XSSFWorkbook xssfworkbook = new XSSFWorkbook(file);
                    sheet = xssfworkbook.GetSheetAt(0);
                }
            }

            System.Collections.IEnumerator rows = sheet.GetRowEnumerator();

            IRow headerRow = sheet.GetRow(0);
            int cellCount = headerRow.LastCellNum;

            for (int j = 0; j < cellCount; j++)
            {
                ICell cell = headerRow.GetCell(j);
                dt.Columns.Add(cell.ToString());
            }

            for (int i = (sheet.FirstRowNum + 1); i <= sheet.LastRowNum; i++)
            {
                IRow row = sheet.GetRow(i);
                DataRow dataRow = dt.NewRow();

                for (int j = row.FirstCellNum; j < cellCount; j++)
                {
                    if (row.GetCell(j) != null)
                        dataRow[j] = row.GetCell(j).ToString();
                }

                dt.Rows.Add(dataRow);
            }
            return dt;
        }
        #endregion

        #region RGB颜色转NPOI颜色
        private static short GetXLColour(HSSFWorkbook workbook, Color SystemColour)
        {
            short s = 0;
            HSSFPalette XlPalette = workbook.GetCustomPalette();
            NPOI.HSSF.Util.HSSFColor XlColour = XlPalette.FindColor(SystemColour.R, SystemColour.G, SystemColour.B);
            if (XlColour == null)
            {
                if (NPOI.HSSF.Record.PaletteRecord.STANDARD_PALETTE_SIZE < 255)
                {
                    XlColour = XlPalette.FindSimilarColor(SystemColour.R, SystemColour.G, SystemColour.B);
                    s = XlColour.Indexed;
                }

            }
            else
                s = XlColour.Indexed;
            return s;
        }
        #endregion

        #region 设置列的对齐方式
        /// <summary>
        /// 设置对齐方式
        /// </summary>
        /// <param name="style"></param>
        /// <returns></returns>
        private static HorizontalAlignment getAlignment(string style)
        {
            switch (style)
            {
                case "center":
                    return HorizontalAlignment.Center;
                case "left":
                    return HorizontalAlignment.Left;
                case "right":
                    return HorizontalAlignment.Right;
                case "fill":
                    return HorizontalAlignment.Fill;
                case "justify":
                    return HorizontalAlignment.Justify;
                case "centerselection":
                    return HorizontalAlignment.CenterSelection;
                case "distributed":
                    return HorizontalAlignment.Distributed;
            }
            return NPOI.SS.UserModel.HorizontalAlignment.General;


        }

        #endregion
        /// <summary>
        /// 返回Excel所有单元格（带类型的）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sourceList"></param>
        /// <returns></returns>
        public static List<TemplateMode> GetTemplateContrainType<T>(T[] sourceList)
        {
            var tempList = new List<TemplateMode>();
            for (int i = 0; i < sourceList.Count(); i++)
            {
                //获取当前行数据对象
                var sourceObj = sourceList[i];
                //获取数据对象的属性字段集合
                var sourceAttri = sourceObj.GetType().GetProperties();
                //循环属性集合，插入对应行
                for (int j = 0; j < sourceAttri.Count(); j++)
                {
                    var unSafeValue = sourceAttri[j].GetValue(sourceObj, null);
                    var enty = new TemplateMode
                    {
                        row = i + 2,
                        cell = j,
                        value = unSafeValue == null ? null : unSafeValue.ToString(),//GetValue(sourceObj).ToString(),
                        FieldType = sourceAttri[j].PropertyType.FullName,
                        FieldName = sourceAttri[j].Name
                    };
                    tempList.Add(enty);
                }
            }
            return tempList;
        }
    }
}
