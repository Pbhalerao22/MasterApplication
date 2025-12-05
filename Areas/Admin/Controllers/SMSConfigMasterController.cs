
using MasterApplication.Areas.Admin.Models;
using MasterApplication.Services;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace MasterApplication.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class SMSConfigMasterController : Controller
    {
        readonly DependancyInjection DI;
        public SMSConfigMasterController(DependancyInjection _DI)
        {
            DI = _DI;
            ViewBag.UserMenuList = DI.GetMenuList(DI.session);
        }

        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public IActionResult GetSms()
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
                                                               strSortType, start, length, "VW_SMSCONFIGMASTER", DI);
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
        public IActionResult Create([Bind] SMSConfigMasterModel sms)
        {
            ActiveUser av = FormsAuthentication.GetCurrentUser(DI.session);

            try
            {

                DataSet dataSet = InsertSMSConfigMaster(sms.PROCESS_TYPE, sms.PROCESS_SUB_TYPE,sms.SMS_CONTENT,sms.OVERDUE_DATE,sms.NUMBER_OF_DAYS, av.UserCode.ToString(), av.ProductName);
                if (dataSet != null && dataSet.Tables[0].Rows.Count > 0)
                {
                    if (dataSet.Tables[0].Rows[0][0].ToString().ToUpper() == "SAVED")
                    {
                        TempData["Message"] = "success|SMS Config Master added successfully";
                        return RedirectToAction("Index");
                    }
                    else if (dataSet.Tables[0].Rows[0][0].ToString().ToUpper() == "EXISTS")
                    {
                        TempData["Message"] = "error|SMS Config Master already exists!";
                    }
                }
                else
                {
                    TempData["Message"] = "error|Error occurred while creating SMS Config Master!";
                }
                return View();
            }
            catch (Exception ex)
            {
                FormsAuthentication.LogException(ex, Request, DI.session, "SMSConfigMaster", "Create", DI.dBAccess);
                TempData["Message"] = "error|Error occurred while creating SMS config master!";
                return View();
            }
        }

        public DataSet InsertSMSConfigMaster(string Process_Type, string Process_Sub_Type,string SMS_Content, string Overdue_Date, string Number_Of_Days, string CreatedBy, string ProductName)
        {
            List<OracleParameter> commands = new List<OracleParameter>();

            commands.Add(new OracleParameter("iv_Code", OracleDbType.Varchar2, -1, System.Data.ParameterDirection.Input));
            commands.Add(new OracleParameter("v_Process_Type", OracleDbType.Varchar2, Process_Type, System.Data.ParameterDirection.Input));
            commands.Add(new OracleParameter("v_Process_Sub_Type", OracleDbType.Varchar2, Process_Sub_Type, System.Data.ParameterDirection.Input));
            commands.Add(new OracleParameter("v_SMS_Content", OracleDbType.Varchar2, SMS_Content, System.Data.ParameterDirection.Input));
            commands.Add(new OracleParameter("v_Overdue_Date", OracleDbType.Varchar2, Overdue_Date, System.Data.ParameterDirection.Input));
            commands.Add(new OracleParameter("v_Number_Of_Days", OracleDbType.Varchar2, Number_Of_Days, System.Data.ParameterDirection.Input));
            commands.Add(new OracleParameter("v_CreatedBy", OracleDbType.Varchar2, CreatedBy, System.Data.ParameterDirection.Input));
            commands.Add(new OracleParameter("v_cursor", OracleDbType.RefCursor, null, System.Data.ParameterDirection.Output));
            DataSet ds = DI.dBAccess.ExecuteDataSet("USP_BOB_ADM_SMSCONFIGMASTER_INSERT", commands);
            return ds;
        }

        public IActionResult Edit(string Code)
        {
            try
            {
                DataSet dataSet = SelectSMSConfigMaster(Code);
                List<SMSConfigMasterModel> SMSMaster = new List<SMSConfigMasterModel>();
                for (int i = 0; i < dataSet.Tables[0].Rows.Count; i++)
                {
                    SMSMaster.Add(new SMSConfigMasterModel
                    {
                        CODE = dataSet.Tables[0].Rows[i]["Code"].ToString(),
                        PROCESS_TYPE = dataSet.Tables[0].Rows[i]["Process_Type"].ToString(),
                        PROCESS_SUB_TYPE = dataSet.Tables[0].Rows[i]["Process_Sub_Type"].ToString(),
                        SMS_CONTENT = dataSet.Tables[0].Rows[i]["SMS_Content"].ToString(),
                        OVERDUE_DATE = dataSet.Tables[0].Rows[i]["Overdue_Date"].ToString(),
                        NUMBER_OF_DAYS = dataSet.Tables[0].Rows[i]["Number_Of_Days"].ToString(),
                        //  LOCKED = Convert.ToBoolean(Convert.ToInt16(dataSet.Tables[0].Rows[i]["Locked"]))
                    });
                }

                return View("Edit", SMSMaster);
            }
            catch (Exception ex)
            {
                FormsAuthentication.LogException(ex, Request, DI.session, "SMSConfigMaster", "Edit(string code)", DI.dBAccess);
                return View();
            }
        }
        [HttpPost]
        public IActionResult Edit([Bind] SMSConfigMasterModel sms)
        {
            try
            {
                ActiveUser av = FormsAuthentication.GetCurrentUser(DI.session);
                DataSet dataSet = UpdateSMSConfigMaster(sms.CODE, sms.PROCESS_TYPE, sms.PROCESS_SUB_TYPE, sms.SMS_CONTENT,sms.OVERDUE_DATE,sms.NUMBER_OF_DAYS, av.UserCode.ToString(), av.BankName, av.ProductName);
                TempData["Message"] = "success|SMS Config Master updated successfully";
            }
            catch (Exception ex)
            {
                FormsAuthentication.LogException(ex, Request, DI.session, "SMSConfigMaster", "Edit", DI.dBAccess);
                TempData["Message"] = "error|Error occurred while updating record";
            }
            return RedirectToAction("Index");
        }

        public DataSet SelectSMSConfigMaster(string UserCode)
        {
            List<OracleParameter> commands = new List<OracleParameter>();

            commands.Add(new OracleParameter("v_Code", OracleDbType.Varchar2, UserCode, System.Data.ParameterDirection.Input));
            commands.Add(new OracleParameter("v_cursor", OracleDbType.RefCursor, null, System.Data.ParameterDirection.Output));

            DataSet ds = DI.dBAccess.ExecuteDataSet("USP_BOB_ADM_SMSCONFIGMASTER_SELECT", commands);
            return ds;
        }

        public DataSet UpdateSMSConfigMaster(string Code, string Process_Type, string Process_Sub_Type, string SMS_Content, string Overdue_Date, string Number_Of_Days, string LastModifiedBy, string strBankName, string strproductName)
        {
            List<OracleParameter> commands = new List<OracleParameter>();

            commands.Add(new OracleParameter("v_Code", OracleDbType.Varchar2, Code, System.Data.ParameterDirection.Input)); ;
            commands.Add(new OracleParameter("v_Process_Type", OracleDbType.Varchar2, Process_Type, System.Data.ParameterDirection.Input));
            commands.Add(new OracleParameter("v_Process_Sub_Type", OracleDbType.Varchar2, Process_Sub_Type, System.Data.ParameterDirection.Input));
            commands.Add(new OracleParameter("v_SMS_Content", OracleDbType.Varchar2, SMS_Content, System.Data.ParameterDirection.Input));
            commands.Add(new OracleParameter("v_Overdue_Date", OracleDbType.Varchar2, Overdue_Date, System.Data.ParameterDirection.Input));
            commands.Add(new OracleParameter("v_Number_Of_Days", OracleDbType.Varchar2, Number_Of_Days, System.Data.ParameterDirection.Input));
            // commands.Add(new OracleParameter("v_Locked", OracleDbType.Int16, Locked ? 1 : 0, System.Data.ParameterDirection.Input));
            commands.Add(new OracleParameter("v_LastModifiedBy", OracleDbType.Int16, Convert.ToInt32(LastModifiedBy), System.Data.ParameterDirection.Input));
            commands.Add(new OracleParameter("v_cursor", OracleDbType.RefCursor, null, System.Data.ParameterDirection.Output));

            DataSet ds = DI.dBAccess.ExecuteDataSet("USP_BOB_ADM_SMSCONFIGMASTER_UPDATE", commands);
            return ds;
        }

        public IActionResult Delete(int Code)
        {
            try
            {
                DataSet dataSet = SmsDelete(Code);
                TempData["Message"] = "success|SMS Config Master deleted successfully";
            }
            catch (Exception ex)
            {
                FormsAuthentication.LogException(ex, Request, DI.session, "SMSConfigMaster", "Delete", DI.dBAccess);
                TempData["Message"] = "error|Error occurred while deleting record";
            }
            return RedirectToAction("Index");
        }
        public DataSet SmsDelete(int Code)
        {
            List<OracleParameter> commands = new List<OracleParameter>();

            commands.Add(new OracleParameter("v_Code", OracleDbType.Int16, Convert.ToInt32(Code), System.Data.ParameterDirection.Input));
            commands.Add(new OracleParameter("v_cursor", OracleDbType.RefCursor, null, System.Data.ParameterDirection.Output));
            DataSet ds = DI.dBAccess.ExecuteDataSet("USP_BOB_ADM_SMSCONFIGMASTER_DELETE", commands);
            return ds;
        }
    }
}
