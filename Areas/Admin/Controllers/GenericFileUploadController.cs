using MasterApplication.Areas.Admin.Models;
using MasterApplication.Services;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using MasterApplication.DAL;

namespace MasterApplication.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class GenericFileUploadController : Controller
    {
        readonly DependancyInjection DI;
        public GenericFileUploadController(DependancyInjection _dependancyInjection)
        {
            DI = _dependancyInjection;
        }
        public IActionResult Index()
        {
            ViewBag.XMLType = Get_XML_UploadFilters();
            return View();
        }
        [HttpPost]
        public IActionResult Index(GenericFileUploadModel model)
        {
            try
            {
                string strFileName = model.FU_GenericFile.FileName;
                string strFilePath = model.TXT_FilePath;

                bool FileExists = CheckFileExists(strFileName,DI.dBAccess);
                if (FileExists)
                {
                    TempData["Message"] = "error|File Already Uploaded !";
                    ViewBag.XMLType = Get_XML_UploadFilters();
                    return View() ;
                }
                if (Directory.Exists(strFilePath))
                {
                    strFilePath = Path.Combine(strFilePath, strFileName);
                    if (System.IO.File.Exists(strFilePath))
                    {
                    TempData["Message"] = "error|File Already Uploaded !";
                    ViewBag.XMLType = Get_XML_UploadFilters();
                    return View() ;
                    }
                    using (var stream = new FileStream(strFilePath, FileMode.Create))
                    {
                        model.FU_GenericFile.CopyTo(stream);
                    }
                    TempData["Message"] = "success|File Uploaded Successfully!";
                }
                else
                {
                    TempData["Message"] = "error|Specified path does not exists!";
                }
            }
            catch (Exception ex)
            {
                FormsAuthentication.LogException(ex, Request, DI.session, "GenericFileUpload", "Index", DI.dBAccess);
            }
            ViewBag.XMLType = Get_XML_UploadFilters();
            return View();
        }
        public List<RupayXMLType> Get_XML_UploadFilters()
        {
            List<OracleParameter> commands = new List<OracleParameter>();
            commands.Add(new OracleParameter("v_cursor", OracleDbType.RefCursor, null, System.Data.ParameterDirection.Output));
            DataSet dataSet = DI.dBAccess.ExecuteDataSet("USP_BOB_GET_XML_FILTERS", commands);
            List<RupayXMLType> repType = new List<RupayXMLType>();
            for (int i = 0; i < dataSet.Tables[0].Rows.Count; i++)
            {
                repType.Add(new RupayXMLType
                {
                    XMLType = dataSet.Tables[0].Rows[i]["XML_Type"].ToString(),
                    Value = dataSet.Tables[0].Rows[i]["XML_Type"].ToString()
                });
            }
            return repType;
        }
        [HttpPost]
        public JsonResult BindPath(string XMLTYPE)
        {
            string strRupayValue = DI.myAppSettings.RupayValue;
            string[] arrValues = strRupayValue.Split(";");
            bool isFullPath = false;
            string strFilePath = "";

            for (int i = 0; i < arrValues.Length; i++)
            {
                isFullPath = arrValues[i].StartsWith(XMLTYPE);
                if (isFullPath)
                {
                    strFilePath = arrValues[i].Split("-")[1].ToString();
                    break;
                }
            }
            return Json(strFilePath);
        }


        public bool CheckFileExists(string strFileName,DBAccess dBAccess)
        {
            List<OracleParameter> lstParams = new List<OracleParameter>();
            lstParams.Add(new OracleParameter("P_PROJECTNAME", OracleDbType.Varchar2, "BOB_RUPAYXMLCLEARINGCONVERTER", ParameterDirection.Input));
            lstParams.Add(new OracleParameter("P_FILENAME", OracleDbType.Varchar2, strFileName, ParameterDirection.Input));
            lstParams.Add(new OracleParameter("P_REFCURSOR", OracleDbType.RefCursor, null, ParameterDirection.Output));

            DataSet ds = dBAccess.ExecuteDataSet("USP_TBL_FILE_UPLOAD_DETAILS_SELECT", lstParams );
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
