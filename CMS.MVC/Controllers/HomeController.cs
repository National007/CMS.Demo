using Serviece.Interface;
using WebModels.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HH.Tools.ExcelFactory;
using HH.Tools.FileFactory;
using CMS.MVC.AutoMapper;
using WebModels;
using System.Data;
using Application;
using Application.Offices.Excel.Model;
using KS.Util.Offices;
using System.IO;
using System.Collections;

namespace CMS.MVC.Controllers
{
    public class HomeController : BaseController
    {
        private IUserRepository _repository;

        private IExcelServices _excel;
        private IFileServices _file;
        public HomeController(
            IUserRepository repository,
            IExcelServices excel,
            IFileServices file)
        {
            this._repository = repository;
            this._excel = excel;
            this._file = file;
        }



        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Main()
        {
            return View();
        }

        public ActionResult LogDetail()
        {
            return View();
        }

        public ActionResult NewsList()
        {
            return View();
        }

        public JsonResult GetList()
        {
            //var list = _repository.GetList();
            var list = _repository.GetListAll();

            //var str = _repository.GetType("嘿嘿");

            var layuiGrid = new LayuiGrid();
            layuiGrid.count = list.Count();
            layuiGrid.data = list;
            return Json(layuiGrid, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Template()
        {
            byte[] file = _file.GetFile("json", "address", ".json");
            return File(file, "text/json", "地址.json");
        }

        /// <summary>
        /// 数据导出
        /// </summary>
        /// <param name="model">条件筛选</param>
        /// <returns></returns>
        public ActionResult ExportExcelData()
        {
            // ExcelExportModel template = new ExcelExportModel();
            var list = _repository.GetList().Select(s =>
            {
                var model = s.ToModel();
                return model;
            });
            var template = _excel.ExportExcelToBytes<UserModels>(list);
            return File(template, "text/xlsx", "测试.xlsx");
        }


        public void ExportExcelData2()
        {
            string newFileName = "模板.xlsx";
            string templdateName = "/resource/template/测试.xlsx";
            DataTable dt = new DataTable();
            var list = _repository.GetList().Select(s =>
            {
                var model = s.ToModel();
                return model;
            });
            if (list.Count() > 0)
            {
                dt = DataHelper.ListToDataTable(list.ToList());

                //需要导出的列
                int arrLen = 5;
                List<TemplateMode> tempList = new List<TemplateMode>();

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    for (int j = 0; j < arrLen; j++)
                    {
                        //if(j>=arrLen-3)
                        string value = dt.Rows[i][j+2].ToString();
                        var enty = new TemplateMode
                        {
                            row = i + 2,
                            cell = j,
                            value = value
                        };
                        tempList.Add(enty);
                    }
                }
                ExcelHelper.ExcelDownload(tempList, templdateName, newFileName);
            }
            else
            {
                Response.Write("<script languge='javascript'>alert('很遗憾,暂无数据');history.back()</script>");
            }


        }

        public ActionResult ImportExecl()
        {
            MessageModel messageModel = new MessageModel();
            HttpPostedFileBase file = Request.Files[0];

            string extension = string.Empty;

            extension = Path.GetExtension(file.FileName);
            if (extension != ".xls" && extension != ".xlsx")
            {
                messageModel.Message = "请上传Excel文件";
                messageModel.Success = false;
                return Json(messageModel);
            }
            var version = ExcelVersion.HSSF;
            if (extension == ".xlsx")
                version = ExcelVersion.XSSF;

            try
            {
                var result = _excel.ImportExcel<UserModels>(file.InputStream, version, true);
                if (result.Success)
                {
                    List<UserModels> importList = result.ResultList as List<UserModels>;
                    var count = 0;
                    importList = importList.Select(s =>
                    {
                        count++;

                        if (string.IsNullOrEmpty(s.Account) || string.IsNullOrEmpty(s.Account.Trim()))
                            throw new Exception("第" + count + "行，账号不能为空！");

                        return s;
                    }).ToList();

                    messageModel.Success = true;
                    messageModel.ResultObject = new
                    {
                        total = importList.Count,
                        rows = importList
                    };
                    return Json(messageModel);

                }
                else
                {
                    messageModel.Message = "导入失败，请检查文件内容是否异常";
                    return Json(messageModel);
                }
            }
            catch (Exception ex)
            {
                messageModel.Message = ex.Message;
                return Json(messageModel);
            }

        }


    }
    /// <summary>
    /// 用于控制器返回处理结果
    /// </summary>
    public class MessageModel
    {
        /// <summary>
        /// 默认构造器
        /// </summary>
        public MessageModel()
        {
            this.Success = true;
        }
        /// <summary>
        /// 是否成功
        /// </summary>
        public bool Success { get; set; }
        /// <summary>
        /// 错误信息
        /// </summary>
        public string Message { get; set; }
        /// <summary>
        /// 可能的返回对象
        /// </summary>
        public IEnumerable Model { get; set; }
        /// <summary>
        /// 用来存放其它model，视需求而定
        /// </summary>
        public IEnumerable OtherModel { get; set; }
        /// <summary>
        /// 可能返回的对象
        /// </summary>
        public object ResultObject { get; set; }
    }

}
