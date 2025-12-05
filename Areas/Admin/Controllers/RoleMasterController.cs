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
    public class RoleMasterController : Controller
    {
        readonly DependancyInjection DI;

        public RoleMasterController(DependancyInjection _DI)
        {
            DI = _DI;
            ViewBag.UserMenuList = DI.GetMenuList(DI.session);
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
            DataSet dataSet = DI.commonClass.GetMasterForGrid_ADM(strSearchValue, strSearchColumn, strSortColumn,
                                                               strSortType, start, length, "VW_BOB_ROLEMASTER",DI);
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
        public IActionResult Create([Bind] RoleMasterModel role)
        {
            ActiveUser av = FormsAuthentication.GetCurrentUser(DI.session);

            try
            {

                DataSet dataSet = InsertRoleMaster(role.ROLENAME, role.ROLEDESCRIPTION, av.UserCode.ToString(), av.ProductName);
                if (dataSet != null && dataSet.Tables[0].Rows.Count > 0)
                {
                    if (dataSet.Tables[0].Rows[0][0].ToString().ToUpper() == "SAVED")
                    {
                        TempData["Message"] = "success|Role added successfully";
                        return RedirectToAction("Index");
                    }
                    else if (dataSet.Tables[0].Rows[0][0].ToString().ToUpper() == "EXISTS")
                    {
                        TempData["Message"] = "error|Role already exists!";
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
                FormsAuthentication.LogException(ex, Request, DI.session, "RoleMaster", "Create", DI.dBAccess);
                TempData["Message"] = "error|Error occurred while creating role!";
                return View();
            }
        }

        public DataSet InsertRoleMaster(string RoleName, string RoleDescription, string CreatedBy, string ProductName)
        {
            List<OracleParameter> commands = new List<OracleParameter>();

            commands.Add(new OracleParameter("iv_Code", OracleDbType.Varchar2, -1, System.Data.ParameterDirection.Input));
            commands.Add(new OracleParameter("v_RoleName", OracleDbType.Varchar2, RoleName, System.Data.ParameterDirection.Input));
            commands.Add(new OracleParameter("v_RoleDesc", OracleDbType.Varchar2, RoleDescription, System.Data.ParameterDirection.Input));
            commands.Add(new OracleParameter("v_CreatedBy", OracleDbType.Varchar2, CreatedBy, System.Data.ParameterDirection.Input));
            commands.Add(new OracleParameter("v_cursor", OracleDbType.RefCursor, null, System.Data.ParameterDirection.Output));
            DataSet ds = DI.dBAccess.ExecuteDataSet_ADM("USP_BOB_ADM_ROLEMASTER_INSERT", commands);
            return ds;
        }

        public IActionResult Edit(string Code)
        {
            try
            {
                DataSet dataSet = SelectRoleMaster(Code);
                List<RoleMasterModel> roleMaster = new List<RoleMasterModel>();
                for (int i = 0; i < dataSet.Tables[0].Rows.Count; i++)
                {
                    roleMaster.Add(new RoleMasterModel
                    {
                        CODE = dataSet.Tables[0].Rows[i]["Code"].ToString(),
                        ROLENAME = dataSet.Tables[0].Rows[i]["RoleName"].ToString(),
                        ROLEDESCRIPTION = dataSet.Tables[0].Rows[i]["RoleDescription"].ToString(),
                        LOCKED = Convert.ToBoolean(Convert.ToInt16(dataSet.Tables[0].Rows[i]["Locked"]))
                    });
                }

                return View("Edit", roleMaster);
            }
            catch (Exception ex)
            {
                FormsAuthentication.LogException(ex, Request, DI.session, "RoleMaster", "Edit(string code)", DI.dBAccess);
                return View();
            }
        }
        [HttpPost]
        public IActionResult Edit([Bind] RoleMasterModel role)
        {
            try
            {
                ActiveUser av = FormsAuthentication.GetCurrentUser(DI.session);
                DataSet dataSet = UpdateRoleMaster(role.CODE, role.ROLENAME, role.ROLEDESCRIPTION, role.LOCKED, av.UserCode.ToString(), av.BankName, av.ProductName);
                TempData["Message"] = "success|Role updated successfully";
            }
            catch (Exception ex)
            {
                FormsAuthentication.LogException(ex, Request, DI.session, "RoleMaster", "Edit", DI.dBAccess);
                TempData["Message"] = "error|Error occurred while updating record";
            }
            return RedirectToAction("Index");
        }

        public DataSet SelectRoleMaster(string UserCode)
        {
            List<OracleParameter> commands = new List<OracleParameter>();

            commands.Add(new OracleParameter("v_Code", OracleDbType.Varchar2, UserCode, System.Data.ParameterDirection.Input));
            commands.Add(new OracleParameter("v_cursor", OracleDbType.RefCursor, null, System.Data.ParameterDirection.Output));

            DataSet ds = DI.dBAccess.ExecuteDataSet_ADM("USP_BOB_ADM_ROLEMASTER_SELECT", commands);
            return ds;
        }

        public DataSet UpdateRoleMaster(string Code, string RoleName, string RoleDescription, bool Locked, string LastModifiedBy, string strBankName, string strproductName)
        {
            List<OracleParameter> commands = new List<OracleParameter>();

            commands.Add(new OracleParameter("v_Code", OracleDbType.Varchar2, Code, System.Data.ParameterDirection.Input)); ;
            commands.Add(new OracleParameter("v_RoleName", OracleDbType.Varchar2, RoleName, System.Data.ParameterDirection.Input));
            commands.Add(new OracleParameter("v_RoleDescription", OracleDbType.Varchar2, RoleDescription, System.Data.ParameterDirection.Input));
            commands.Add(new OracleParameter("v_Locked", OracleDbType.Int16, Locked ? 1 : 0, System.Data.ParameterDirection.Input));
            commands.Add(new OracleParameter("v_LastModifiedBy", OracleDbType.Int16, Convert.ToInt32(LastModifiedBy), System.Data.ParameterDirection.Input));
            commands.Add(new OracleParameter("v_cursor", OracleDbType.RefCursor, null, System.Data.ParameterDirection.Output));

            DataSet ds = DI.dBAccess.ExecuteDataSet_ADM("USP_BOB_ADM_ROLEMASTER_UPDATE", commands);
            return ds;
        }

        public IActionResult Delete(int Code)
        {
            try
            {
                ActiveUser av = FormsAuthentication.GetCurrentUser(DI.session);
                DataSet dataSet = RoleDelete(Code,Convert.ToInt32(av.UserCode));
                TempData["Message"] = "success|Role deleted successfully";
            }
            catch (Exception ex)
            {
                FormsAuthentication.LogException(ex, Request, DI.session, "RoleMaster", "Delete", DI.dBAccess);
                TempData["Message"] = "error|Error occurred while deleting record";
            }
            return RedirectToAction("Index");
        }
        public DataSet RoleDelete(int Code,int deletedby)
        {
            List<OracleParameter> commands = new List<OracleParameter>();

            commands.Add(new OracleParameter("v_Code", OracleDbType.Int16, Convert.ToInt32(Code), System.Data.ParameterDirection.Input));
            commands.Add(new OracleParameter("V_DELETED_BY", OracleDbType.Int32, deletedby, System.Data.ParameterDirection.Input));
            commands.Add(new OracleParameter("v_cursor", OracleDbType.RefCursor, null, System.Data.ParameterDirection.Output));
            DataSet ds = DI.dBAccess.ExecuteDataSet_ADM("USP_BOB_ADM_ROLEMASTER_DELETE", commands);
            return ds;
        }
    }
}
