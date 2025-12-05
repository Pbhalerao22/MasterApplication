
using MasterApplication.Areas.Admin.Models;
using MasterApplication.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
namespace MasterApplication.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class EmailConfigMasterController : Controller
    {
        readonly DependancyInjection DI;
        public EmailConfigMasterController(DependancyInjection _DI)
        {
            DI = _DI;
            ViewBag.UserMenuList = DI.GetMenuList(DI.session);

        }

        public IActionResult Index()
        {
            return View();
        }



        [HttpPost]
        public IActionResult GetEmail()
        {
            string JsonString = Request.Form.Keys.FirstOrDefault();
            JObject JArray = JObject.Parse(JsonString);

            int start = Convert.ToInt16(JArray["PageNo"].ToString());
            int length = Convert.ToInt16(JArray["PageSize"].ToString());
            string strSearchColumn = JArray["SearchColumn"].ToString();
            string strSearchValue = JArray["SearchValue"].ToString();
            string strSortColumn = JArray["SortColumn"].ToString();
            string strSortType = JArray["SortType"].ToString();

            int pageSize = length != null ? Convert.ToInt32(length) : 0;
            int skip = start != null ? Convert.ToInt32(start * pageSize) : 0;
            int recordsTotal = 0;
            DataSet dataSet = DI.commonClass.GetMasterForGrid(strSearchValue, strSearchColumn, strSortColumn,
                                                               strSortType, start, length, "VW_EMAILCONFIGMASTER", DI);
            int recordsFiltered = Convert.ToUInt16(dataSet.Tables[0].Rows[0]["TotalRecords"]);
            int TotalRecords = dataSet.Tables[1].Rows.Count;
            string json = JsonConvert.SerializeObject(dataSet.Tables[1], Formatting.Indented);
            var jsonData = new
            {
                //draw = start=start+1, 
                recordsFiltered = recordsFiltered,
                recordsTotal = TotalRecords,
                data = json
                //data = dataSet.Tables[0]
            };
            return Ok(jsonData);
        }

        public IActionResult Create()
        {
            //ViewBag.SecurityMaster = GetSecurityQuestions();
            return View();
        }


        [HttpPost]
        public IActionResult Create(EmailConfigMasterModel email)
        {
            ActiveUser av = FormsAuthentication.GetCurrentUser(DI.session);

            try
            {
                #region Email File
                string strEmailFileName = email.EmailContentFile.FileName;
                string strProcessType = email.PROCESS_TYPE;
                string strProcessSubType = email.PROCESS_SUB_TYPE;
                string strEmailTemplateCommonPath = DI.myAppSettings.EmailTemplateCommonPath;
                DataSet dataSet = new DataSet();

                string strProcessTypePath = Path.Combine(strEmailTemplateCommonPath, strProcessType);
                if (!Directory.Exists(strProcessTypePath))
                {
                    Directory.CreateDirectory(strProcessTypePath);
                }

                string strProcessSubTypePath = Path.Combine(strProcessTypePath, strProcessSubType);
                if (!Directory.Exists(strProcessSubTypePath))
                {
                    Directory.CreateDirectory(strProcessSubTypePath);
                }
                string strEmailFile = Path.Combine(strProcessSubTypePath, strEmailFileName);
                using (var stream = new FileStream(strEmailFile, FileMode.Create))
                {
                    email.EmailContentFile.CopyTo(stream);
                }
                #endregion

                #region Attachment File
                string strCurrFileName = "";
                string strAttachmentPath = "";
                string strAttachmentName = "";
                if (email.AttachmentFile != null)
                {
                    strAttachmentPath = Path.Combine(strProcessSubTypePath, "Attachment");

                    if (!Directory.Exists(strAttachmentPath))
                    {
                        Directory.CreateDirectory(strAttachmentPath);
                    }

                    foreach (var file in email.AttachmentFile)
                    {
                        strCurrFileName = file.FileName;
                        if (strAttachmentName != "")
                        {
                            strAttachmentName = strAttachmentName + "," + strCurrFileName;
                        }
                        else
                        {
                            strAttachmentName = strCurrFileName;
                        }
                        string strAttachmentFile = Path.Combine(strAttachmentPath, strCurrFileName);
                        using (var stream = new FileStream(strAttachmentFile, FileMode.Create))
                        {
                            file.CopyTo(stream);
                        }

                    }
                }
                #endregion

                dataSet = InsertEmailConfigMaster(email.PROCESS_TYPE, email.PROCESS_SUB_TYPE, strEmailFile, strAttachmentPath,
                        strAttachmentName, av.UserCode.ToString(), av.ProductName);

                if (dataSet != null && dataSet.Tables[0].Rows.Count > 0)
                {
                    if (dataSet.Tables[0].Rows[0][0].ToString().ToUpper() == "SAVED")
                    {
                        TempData["Message"] = "success|Email Config Master added successfully";
                        return RedirectToAction("Index");
                    }
                    else if (dataSet.Tables[0].Rows[0][0].ToString().ToUpper() == "EXISTS")
                    {
                        TempData["Message"] = "error|Email Config Master already exists!";
                    }
                }
                else
                {
                    TempData["Message"] = "error|Error occurred while creating Email Config Master!";
                }
                return View();
            }

            catch (Exception ex)
            {
                FormsAuthentication.LogException(ex, Request, DI.session, "EmailConfigMaster", "Create", DI.dBAccess);
                TempData["Message"] = "error|Error occurred while creating Email! config master";
                return View();
            }
        }


        public DataSet InsertEmailConfigMaster(string Process_Type, string Process_Sub_Type, string Email_Content, string Attachment_Path,
             string Attachment_Name, string CreatedBy, string ProductName)
        {
            List<OracleParameter> commands = new List<OracleParameter>();

            commands.Add(new OracleParameter("iv_Code", OracleDbType.Varchar2, -1, System.Data.ParameterDirection.Input));
            commands.Add(new OracleParameter("v_Process_Type", OracleDbType.Varchar2, Process_Type, System.Data.ParameterDirection.Input));
            commands.Add(new OracleParameter("v_Process_Sub_Type", OracleDbType.Varchar2, Process_Sub_Type, System.Data.ParameterDirection.Input));
            commands.Add(new OracleParameter("v_Email_Content", OracleDbType.Varchar2, Email_Content, System.Data.ParameterDirection.Input));
            commands.Add(new OracleParameter("v_Attachment_Path", OracleDbType.Varchar2, Attachment_Path, System.Data.ParameterDirection.Input));
            commands.Add(new OracleParameter("v_Attachment_Name", OracleDbType.Varchar2, Attachment_Name, System.Data.ParameterDirection.Input));
            commands.Add(new OracleParameter("v_CreatedBy", OracleDbType.Varchar2, CreatedBy, System.Data.ParameterDirection.Input));
            commands.Add(new OracleParameter("v_cursor", OracleDbType.RefCursor, null, System.Data.ParameterDirection.Output));
            DataSet ds = DI.dBAccess.ExecuteDataSet("USP_BOB_ADM_EMAILCONFIGMASTER_INSERT", commands);
            return ds;
        }

        public IActionResult Edit(string Code)
        {
            try
            {
                DataSet dataSet = SelectEmailConfigMaster(Code);
                List<EmailConfigMasterModel> emailmaster = new List<EmailConfigMasterModel>();
                for (int i = 0; i < dataSet.Tables[0].Rows.Count; i++)
                {
                    emailmaster.Add(new EmailConfigMasterModel
                    {
                        CODE = dataSet.Tables[0].Rows[i]["Code"].ToString(),
                        PROCESS_TYPE = dataSet.Tables[0].Rows[i]["Process_Type"].ToString(),
                        PROCESS_SUB_TYPE = dataSet.Tables[0].Rows[i]["Process_Sub_Type"].ToString(),
                        EMAIL_CONTENT = dataSet.Tables[0].Rows[i]["Email_Content"].ToString(),
                        ATTACHMENT_PATH = dataSet.Tables[0].Rows[i]["Attachment_Path"].ToString(),
                        ATTACHMENT_NAME = dataSet.Tables[0].Rows[i]["Attachment_Name"].ToString(),

                    });
                }

                return View("Edit", emailmaster);
            }
            catch (Exception ex)
            {
                FormsAuthentication.LogException(ex, Request, DI.session, "EmailConfigMaster", "Edit(string code)", DI.dBAccess);
                return View();
            }
        }
        [HttpPost]
        public IActionResult Edit([Bind] EmailConfigMasterModel email)
        {
            try
            {
                ActiveUser av = FormsAuthentication.GetCurrentUser(DI.session);
                DataSet dataSet = UpdateEmailConfigMaster(email.CODE, email.PROCESS_TYPE, email.PROCESS_SUB_TYPE, email.EMAIL_CONTENT, email.ATTACHMENT_PATH, email.ATTACHMENT_NAME, av.UserCode.ToString(), av.BankName, av.ProductName);
                TempData["Message"] = "success|EMAIL Config Master updated successfully";
            }
            catch (Exception ex)
            {
                FormsAuthentication.LogException(ex, Request, DI.session, "EmailConfigMaster", "Edit", DI.dBAccess);
                TempData["Message"] = "error|Error occurred while updating record";
            }
            return RedirectToAction("Index");
        }

        public DataSet SelectEmailConfigMaster(string UserCode)
        {
            List<OracleParameter> commands = new List<OracleParameter>();

            commands.Add(new OracleParameter("v_Code", OracleDbType.Varchar2, UserCode, System.Data.ParameterDirection.Input));
            commands.Add(new OracleParameter("v_cursor", OracleDbType.RefCursor, null, System.Data.ParameterDirection.Output));

            DataSet ds = DI.dBAccess.ExecuteDataSet("USP_BOB_ADM_EMAILCONFIGMASTER_SELECT", commands);
            return ds;
        }

        public DataSet UpdateEmailConfigMaster(string Code, string Process_Type, string Process_Sub_Type, string Email_Content, string Attachment_Path, string Attachment_Name, string LastModifiedBy, string strBankName, string strproductName)
        {
            List<OracleParameter> commands = new List<OracleParameter>();

            commands.Add(new OracleParameter("v_Code", OracleDbType.Varchar2, Code, System.Data.ParameterDirection.Input)); ;
            commands.Add(new OracleParameter("v_Process_Type", OracleDbType.Varchar2, Process_Type, System.Data.ParameterDirection.Input));
            commands.Add(new OracleParameter("v_Process_Sub_Type", OracleDbType.Varchar2, Process_Sub_Type, System.Data.ParameterDirection.Input));
            commands.Add(new OracleParameter("v_Email_Content", OracleDbType.Varchar2, Email_Content, System.Data.ParameterDirection.Input));
            commands.Add(new OracleParameter("v_Attachment_Path", OracleDbType.Varchar2, Attachment_Path, System.Data.ParameterDirection.Input));
            commands.Add(new OracleParameter("v_Attachment_Name", OracleDbType.Varchar2, Attachment_Name, System.Data.ParameterDirection.Input));
            commands.Add(new OracleParameter("v_LastModifiedBy", OracleDbType.Int16, Convert.ToInt32(LastModifiedBy), System.Data.ParameterDirection.Input));
            commands.Add(new OracleParameter("v_cursor", OracleDbType.RefCursor, null, System.Data.ParameterDirection.Output));

            DataSet ds = DI.dBAccess.ExecuteDataSet("USP_BOB_ADM_EMAILCONFIGMASTER_UPDATE", commands);
            return ds;
        }

        public IActionResult Delete(int Code)
        {
            try
            {
                DataSet dataSet = EmailDelete(Code);
                TempData["Message"] = "success|Email Config Master deleted successfully";
            }
            catch (Exception ex)
            {
                FormsAuthentication.LogException(ex, Request, DI.session, "EmailConfigMaster", "Delete", DI.dBAccess);
                TempData["Message"] = "error|Error occurred while deleting record";
            }
            return RedirectToAction("Index");
        }
        public DataSet EmailDelete(int Code)
        {
            List<OracleParameter> commands = new List<OracleParameter>();

            commands.Add(new OracleParameter("v_Code", OracleDbType.Int16, Convert.ToInt32(Code), System.Data.ParameterDirection.Input));
            commands.Add(new OracleParameter("v_cursor", OracleDbType.RefCursor, null, System.Data.ParameterDirection.Output));
            DataSet ds = DI.dBAccess.ExecuteDataSet("USP_BOB_ADM_EMAILCONFIGMASTER_DELETE", commands);
            return ds;
        }
    }
}