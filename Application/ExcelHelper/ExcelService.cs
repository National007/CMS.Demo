using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.ComponentModel;
using System.Configuration;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using NPOI.HSSF.UserModel;
using System.ComponentModel.DataAnnotations;

namespace Application.ExcelHelper
{
    public enum ExcelVersion
    {
        XSSF,//2007版本
        HSSF,//2003版本

    }
    public class ExcelService : IExcelService
    {
        private readonly string savePath = ConfigurationManager.AppSettings["ExcelSavePath"];
        //private const  string numerical = "int, double, decimal" ;

        #region 导入
        public ExcelResultModel LoadExcel<T>(Byte[] bytes, string extend, bool isContainNull)
        {
            Stream s = new MemoryStream(bytes);
            return LoadExcel<T>(s, extend, isContainNull);
        }
        public ExcelResultModel LoadExcel<T>(Stream file, string extend, bool isContainNull)
        {
            ExcelResultModel result = new ExcelResultModel();
            List<T> list = new List<T>();
            IWorkbook workbook = null;
            if (file == null)
            {
                throw new ArgumentNullException("File");
            }
            if (extend == ".xlsx")
            {
                workbook = new XSSFWorkbook(file);//2007版本
            }
            else
            {
                workbook = new HSSFWorkbook(file);
            }

            ISheet sheet = workbook.GetSheetAt(0);

            IRow fieldRow = sheet.GetRow(1);
            string[] fieldArray = new string[fieldRow.LastCellNum];
            for (int i = 0; i < fieldRow.LastCellNum; i++)
            {
                ICell fieldCell = fieldRow.GetCell(i);

                fieldArray[i] = fieldCell != null ? fieldCell.StringCellValue.ToLower() : "";
                //if (fieldCell != null)
                //    fieldArray[i] = fieldCell.StringCellValue.ToLower();
            }

            List<String> fieldList = fieldArray.ToList();

            string fieldName = string.Empty;
            try
            {

                for (int i = 2; i <= sheet.LastRowNum; i++)
                {
                    result.Row = i;
                    IRow row = sheet.GetRow(i);
                    Type type = typeof(T);
                    var info = type.GetConstructor(new Type[] { });
                    T node = (T)info.Invoke(new object[] { });

                    foreach (var item in type.GetProperties(BindingFlags.Public | BindingFlags.Instance))
                    {
                        fieldName = item.Name.ToLower();
                        item.GetCustomAttributes(typeof(KeyAttribute), false);

                        int index = fieldList.IndexOf(fieldName);
                        if (index == -1)
                        {
                            continue;
                        }
                        ICell cell = row.GetCell(index);
                        if (cell != null)
                        {
                            result.Cell = index;
                            Type nodeType = item.PropertyType;

                            object value = GetCellValue(cell, nodeType);
                            if (value != null || isContainNull)
                                item.SetValue(node, value, null);
                            else
                            {
                                result.Success = false;
                                result.ErrorList.Add(string.Format("数据为空或者数据类型错误,出现在第{0}行 第{1}列,请将类型转转为{2}类型", result.Row + 1, result.Cell + 1, GetDescriptionByDataType(nodeType)));
                            }
                        }


                    }
                    list.Add(node);
                }

            }
            catch (Exception e)
            {
                result.Success = false;
                result.ErrorMessage = e.Message.ToString();
            }

            result.List = list;
            return result;
        }
        

        public ExcelResultModel LoadExcel(Type type, ISheet sheet, bool isContainNull)
        {
            ExcelResultModel result = new ExcelResultModel();
            List<Object> list = new List<Object>();

            IRow fieldRow = sheet.GetRow(1);
            string[] fieldArray = new string[fieldRow.LastCellNum];
            for (int i = 0; i < fieldRow.LastCellNum; i++)
            {
                ICell fieldCell = fieldRow.GetCell(i);

                fieldArray[i] = fieldCell != null ? fieldCell.StringCellValue.ToLower() : "";
            }

            List<String> fieldList = fieldArray.ToList();

            string fieldName = string.Empty;
            int count = 0;
            try
            {

                for (int i = 2; i <= sheet.LastRowNum; i++)
                {
                    result.Row = i;
                    IRow row = sheet.GetRow(i);

                    //Type type = typeof(T);
                    //var info = type.GetConstructor(new Type[] { });
                    //var node = (type)info.Invoke(new object[] { });
                    var node = Activator.CreateInstance(type);
                    count = 0;
                    foreach (var item in type.GetProperties(BindingFlags.Public | BindingFlags.Instance))
                    {
                        fieldName = item.Name.ToLower();
                        item.GetCustomAttributes(typeof(KeyAttribute), false);

                        int index = fieldList.IndexOf(fieldName);
                        if (index == -1)
                        {
                            continue;
                        }
                        ICell cell = row.GetCell(index);
                        if (cell != null)
                        {

                            result.Cell = index;
                            Type nodeType = item.PropertyType;

                            object value = GetCellValue(cell, nodeType);
                            if (value != null || isContainNull)
                            {
                                item.SetValue(node, value, null);
                                count++;
                            }
                            else
                            {
                                result.Success = false;
                                result.ErrorList.Add(string.Format("数据为空或者数据类型错误,出现在第{0}行 第{1}列,请将类型转转为{2}类型", result.Row + 1, result.Cell + 1, GetDescriptionByDataType(nodeType)));
                            }
                        }


                    }
                    if(count>0)
                        list.Add(node);
                }

            }
            catch (Exception e)
            {
                result.Success = false;
                result.ErrorMessage = e.Message.ToString();
            }

            result.List = list;
            return result;
        }

        private object GetCellValue(ICell cell, Type nodeType)
        {
            string nodeTypeStr = nodeType.Name;

            if (nodeType.IsGenericType)
            {
                var argument = nodeType.GetGenericArguments();
                nodeTypeStr = nodeType.GetGenericArguments()[0].Name;
            }
            try
            {
                switch (cell.CellType)
                {
                    case CellType.String:
                        if (nodeTypeStr == "String")
                            return cell.StringCellValue;
                        break;
                    case CellType.Boolean:
                        if (nodeTypeStr == "Bool")
                            return cell.BooleanCellValue;
                        break;
                    case CellType.Numeric:
                        switch (nodeTypeStr)
                        {
                            case "Int64":
                                return (Int64)cell.NumericCellValue;
                            case "Int32":
                                return (Int32)cell.NumericCellValue;
                            case "Int16":
                                return (Int16)cell.NumericCellValue;
                            case "Decimal":
                                return (decimal)cell.NumericCellValue;
                            case "Double":
                                return (double)cell.NumericCellValue;
                            case "DateTime":
                                return cell.DateCellValue;
                            case "String":
                                return cell.NumericCellValue.ToString();
                        }
                        break;
                    case CellType.Blank:
                    case CellType.Formula:
                    case CellType.Unknown:
                    case CellType.Error:
                        break;

                }
            }
            catch
            {
            }
            return null;
        }
        private string GetDescriptionByDataType(Type nodeType)
        {
            string typeName = nodeType.Name;
            if (nodeType.IsGenericType)
                typeName = nodeType.GetGenericArguments()[0].Name;

            switch (typeName)
            {
                case "Int32":
                case "Decimal":
                case "Double":
                    return "数值";
                case "DateTime":
                    return "时间";
                case "String":
                    return "字符串";
                case "Bool":
                case "Boolean":
                    return "布尔";
                default:
                    break;
            }
            return "";
        }

        #endregion


        #region 导出


        #region 暂不用

        ///// <summary>
        ///// 导出
        ///// </summary>
        ///// <typeparam name="T"></typeparam>
        ///// <param name="list"></param>
        //public string ExportExcel<T>(IEnumerable<T> list)
        //{
        //    int flag = 0;
        //    string saveName = String.Format("{0}{1}.xls", DateTime.Now.ToString(), new Random().Next(1000, 10000).ToString());
        //    string path = Path.Combine(savePath, saveName);
        //    Type type = typeof(T);

        //    IWorkbook work = null;
        //    work = new XSSFWorkbook();

        //    ISheet sheet = work.CreateSheet("sheet1");
        //    IRow rowByName = sheet.CreateRow(1);
        //    IRow rowByField = sheet.CreateRow(2);

        //    try
        //    {
        //        foreach (PropertyInfo property in type.GetProperties())
        //        {
        //            string name = GetDisplayName(property);
        //            ICell nameCell = rowByName.CreateCell(flag);
        //            nameCell.SetCellValue(name);

        //            ICell fieldCell = rowByField.CreateCell(flag);
        //            fieldCell.SetCellValue(property.Name);
        //            flag++;

        //        }
        //        foreach (T item in list)
        //        {
        //            Type nodeType = item.GetType();
        //            IRow valueRow = sheet.CreateRow(flag);
        //            int nodeFlag = 0;
        //            foreach (PropertyInfo propertyByNode in nodeType.GetProperties())
        //            {
        //                ICell cell = valueRow.CreateCell(nodeFlag);

        //                SetCellValue(cell, propertyByNode.PropertyType, propertyByNode.GetValue(item));
        //            }
        //            flag++;
        //        }

        //        using (FileStream fsWrite = File.OpenWrite(path))
        //        {
        //            work.Write(fsWrite);
        //        }
        //    }
        //    catch
        //    {
        //        path = "";
        //    }

        //    return path;
        //} 
        #endregion
        /// <summary>
        /// 导出Excel
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <returns></returns>
        public string ExportExcel<T>(IEnumerable<T> list, ExcelVersion version)
        {
            List<String> fields = new List<String>();
            Type type = typeof(T);
            foreach (PropertyInfo property in type.GetProperties())
            {
                fields.Add(property.Name);
            }


            return ExportExcel(list, fields.ToArray(), version);
        }

        public string ExportExcel<T>(IEnumerable<T> list, string[] fields, ExcelVersion version)
        {
            string saveName = String.Format("{0}{1}.xls", DateTime.Now.ToString(), new Random().Next(1000, 10000).ToString());
            string path = Path.Combine(savePath, saveName);

            Type type = typeof(T);

            IWorkbook wk = null;

            if (version == ExcelVersion.XSSF)
                wk = new XSSFWorkbook();
            else
                wk = new HSSFWorkbook();

            ISheet sheet = wk.CreateSheet("sheet1");
            //创建头部
            IRow rowByName = sheet.CreateRow(1);
            IRow rowByField = sheet.CreateRow(2);
            try
            {
                for (int i = 0; i < fields.Length; i++)
                {
                    PropertyInfo property = type.GetProperty(fields[i]);
                    ICell cellByName = rowByName.CreateCell(i);
                    ICell cellByField = rowByField.CreateCell(i);

                    cellByName.SetCellValue(GetDisplayName(property));
                    cellByField.SetCellValue(fields[i]);

                }
                //内容区域
                int flag = 3;
                foreach (T item in list)
                {
                    IRow contentRow = sheet.CreateRow(flag);
                    Type modelType = item.GetType();
                    for (int i = 0; i < fields.Length; i++)
                    {
                        PropertyInfo property = modelType.GetProperty(fields[i]);

                        ICell cell = contentRow.CreateCell(i);
                        SetCellValue(cell, property.PropertyType, property.GetValue(item));
                    }
                    flag++;
                }
                CreateFolder(path);
                using (FileStream fsWrite = File.OpenWrite(path))
                {
                    wk.Write(fsWrite);
                }

            }
            catch
            {
                path = "";

            }
            return path;
        }


        /// <summary>
        /// 为单元格赋值
        /// </summary>
        /// <param name="cell"></param>
        /// <param name="nodeType"></param>
        /// <param name="value"></param>
        private void SetCellValue(ICell cell, Type nodeType, object value)
        {
            if (value == null)
            {
                cell.SetCellType(CellType.Blank);
                return;
            }
            string nodeTypeStr = nodeType.Name;
            if (nodeType.IsGenericType)
            {
                nodeTypeStr = nodeType.GetGenericArguments()[0].Name;
            }
            switch (nodeTypeStr.ToLower())
            {
                case "int16":
                case "int32":
                case "int64":
                case "decimal":
                case "double":
                    cell.SetCellValue(Convert.ToDouble(value));
                    break;
                case "datetime":
                    cell.SetCellValue(Convert.ToDateTime(value));
                    break;
                case "string":
                    cell.SetCellValue(value.ToString());
                    break;
                case "bool":
                    cell.SetCellValue((bool)value);
                    break;
            }

        }

        /// <summary>
        /// 获取display特性的值
        /// </summary>
        /// <param name="property"></param>
        /// <returns></returns>
        private string GetDisplayName(PropertyInfo property)
        {
            var attributes = property.GetCustomAttributes();

            foreach (var item in attributes)
            {
                if (item is DisplayNameAttribute)
                {
                    return (item as DisplayNameAttribute).DisplayName;
                }
            }
            return "";
        }
        #endregion


        private void CreateFolder(string path)
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
        }
    }
}
