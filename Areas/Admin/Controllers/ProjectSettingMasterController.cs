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
    public class ProjectSettingMasterController : Controller
    {
        readonly DependancyInjection DI;
        public ProjectSettingMasterController(DependancyInjection _dependancyInjection)
        {
            DI = _dependancyInjection;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult GetRoles()
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
                                                               strSortType, start, length, "VW_USP_PROJECTSETTING", DI);
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
        public IActionResult Create([Bind] ProjectSettingMasterModel role)
        {
            ActiveUser av = FormsAuthentication.GetCurrentUser(DI.session);

            try
            {

                DataSet dataSet = InsertProjectSettingMaster(role.PROJECT_NAME, role.KEYNAME, role.KEYVALUE, av.UserCode.ToString(), av.ProductName);
                if (dataSet != null && dataSet.Tables[0].Rows.Count > 0)
                {
                    if (dataSet.Tables[0].Rows[0][0].ToString().ToUpper() == "SAVED")
                    {
                        TempData["Message"] = "success|Project Setting added successfully";
                        return RedirectToAction("Index");
                    }
                    else if (dataSet.Tables[0].Rows[0][0].ToString().ToUpper() == "EXISTS")
                    {
                        TempData["Message"] = "error|Project Setting already exists!";

                    }

                }
                else
                {
                    TempData["Message"] = "error|Error occurred while creating role!";

                }
                return View();
            }
            catch (Exception ex)
            {
                FormsAuthentication.LogException(ex, Request, DI.session, "ProjectSettingMaster", "Create", DI.dBAccess);
                TempData["Message"] = "error|Error occurred while creating role!";
                return View();
            }

        }

        public DataSet InsertProjectSettingMaster(string Project_Name, string KeyName, string KeyValue, string CreatedBy, string ProductName)
        {
            List<OracleParameter> commands = new List<OracleParameter>();

            commands.Add(new OracleParameter("iv_Code", OracleDbType.Varchar2, -1, System.Data.ParameterDirection.Input));
            commands.Add(new OracleParameter("v_PROJECT_NAME", OracleDbType.Varchar2, Project_Name, System.Data.ParameterDirection.Input));
            commands.Add(new OracleParameter("v_KEYNAME", OracleDbType.Varchar2, KeyName, System.Data.ParameterDirection.Input));
            commands.Add(new OracleParameter("V_KEYVALUE", OracleDbType.Varchar2, KeyValue, System.Data.ParameterDirection.Input));
            commands.Add(new OracleParameter("v_CreatedBy", OracleDbType.Varchar2, CreatedBy, System.Data.ParameterDirection.Input));
            commands.Add(new OracleParameter("v_cursor", OracleDbType.RefCursor, null, System.Data.ParameterDirection.Output));
            DataSet ds = DI.dBAccess.ExecuteDataSet("USP_PROJECTSETTING_INSERT", commands);
            return ds;
        }
        public IActionResult Edit(string Code)
        {
            try
            {
                DataSet dataSet = SelectProjectSettingMaster(Code);
                List<ProjectSettingMasterModel> roleMaster = new List<ProjectSettingMasterModel>();
                for (int i = 0; i < dataSet.Tables[0].Rows.Count; i++)
                {
                    roleMaster.Add(new ProjectSettingMasterModel
                    {
                        CODE = dataSet.Tables[0].Rows[i]["Code"].ToString(),
                        PROJECT_NAME = dataSet.Tables[0].Rows[i]["Project_Name"].ToString(),
                        KEYNAME = dataSet.Tables[0].Rows[i]["KeyName"].ToString(),
                        KEYVALUE = dataSet.Tables[0].Rows[i]["KeyValue"].ToString(),
                        LOCKED = Convert.ToBoolean(Convert.ToInt16(dataSet.Tables[0].Rows[i]["Locked"]))
                    });
                }

                return View("Edit", roleMaster);
            }
            catch (Exception ex)
            {
                FormsAuthentication.LogException(ex, Request, DI.session, "ProjectSettingMaster", "Edit(string code)", DI.dBAccess);
                return View();
            }
        }
        [HttpPost]
        public IActionResult Edit([Bind] ProjectSettingMasterModel role)
        {
            try
            {
                ActiveUser av = FormsAuthentication.GetCurrentUser(DI.session);
                DataSet dataSet = UpdateProjectSettingMaster(role.CODE, role.PROJECT_NAME, role.KEYNAME, role.KEYVALUE, Convert.ToBoolean(role.LOCKED), av.UserCode.ToString(), av.BankName, av.ProductName);
                TempData["Message"] = "success|Project Setting Master updated successfully";
            }
            catch (Exception ex)
            {
                FormsAuthentication.LogException(ex, Request, DI.session, "ProjectSettingMaster", "Edit", DI.dBAccess);
                TempData["Message"] = "error|Error occurred while updating record";
            }
            return RedirectToAction("Index");
        }

        public DataSet SelectProjectSettingMaster(string UserCode)
        {
            List<OracleParameter> commands = new List<OracleParameter>();
            commands.Add(new OracleParameter("v_Code", OracleDbType.Varchar2, UserCode, System.Data.ParameterDirection.Input));
            commands.Add(new OracleParameter("v_cursor", OracleDbType.RefCursor, null, System.Data.ParameterDirection.Output));
            DataSet ds = DI.dBAccess.ExecuteDataSet("USP_PROJECTSETTING_SELECT", commands);
            return ds;
        }

        public DataSet UpdateProjectSettingMaster(string Code, string Project_Name, string KeyName, string KeyValue, bool Locked, string LastModifiedBy, string strBankName, string strproductName)
        {
            List<OracleParameter> commands = new List<OracleParameter>();

            commands.Add(new OracleParameter("v_Code", OracleDbType.Varchar2, Code, System.Data.ParameterDirection.Input)); ;
            commands.Add(new OracleParameter("v_PROJECT_NAME", OracleDbType.Varchar2, Project_Name, System.Data.ParameterDirection.Input));
            commands.Add(new OracleParameter("v_KEYNAME", OracleDbType.Varchar2, KeyName, System.Data.ParameterDirection.Input));
            commands.Add(new OracleParameter("v_KEYVALUE", OracleDbType.Varchar2, KeyValue, System.Data.ParameterDirection.Input));
            commands.Add(new OracleParameter("v_LOCKED", OracleDbType.Int16, Locked ? 1 : 0, Locked, System.Data.ParameterDirection.Input));
            commands.Add(new OracleParameter("v_LastModifiedBy", OracleDbType.Int16, Convert.ToInt32(LastModifiedBy), System.Data.ParameterDirection.Input));
            commands.Add(new OracleParameter("v_cursor", OracleDbType.RefCursor, null, System.Data.ParameterDirection.Output));
            DataSet ds = DI.dBAccess.ExecuteDataSet("USP_PROJECTSETTING_UPDATE", commands);
            return ds;

        }

        public IActionResult Delete(string Code)
        {
            try
            {
                DataSet dataSet = ProjectSettingDelete(Code);
                TempData["Message"] = "success|Project Setting Master deleted successfully";
            }
            catch (Exception ex)
            {
                FormsAuthentication.LogException(ex, Request, DI.session, "ProjectSettingMaster", "Delete", DI.dBAccess);
                TempData["Message"] = "error|Error occurred while deleting record";
            }
            return RedirectToAction("Index");
        }
        public DataSet ProjectSettingDelete(string Code)
        {
            List<OracleParameter> commands = new List<OracleParameter>();
            commands.Add(new OracleParameter("v_Code", OracleDbType.Int16, Convert.ToInt32(Code), System.Data.ParameterDirection.Input));
            commands.Add(new OracleParameter("v_cursor", OracleDbType.RefCursor, null, System.Data.ParameterDirection.Output));
            DataSet ds = DI.dBAccess.ExecuteDataSet("USP_PROJECTSETTING_DELETE", commands);
            return ds;
        }
    }
}
