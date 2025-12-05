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
    public class UserMasterController : Controller
    {
        readonly DependancyInjection DI;

        public UserMasterController(DependancyInjection _DI)
        {
            DI = _DI;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public IActionResult GetUser()
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
                                                               strSortType, start, length, "VW_BOB_USERMASTER", DI);
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
            ViewBag.SecurityMaster = GetSecurityQuestions();
            return View();
        }
        [HttpPost]
        public IActionResult Create([Bind] UserMasterModel user)
        {
            ActiveUser av = FormsAuthentication.GetCurrentUser(DI.session);
            if (user.ISADUSER.Equals("true"))
            {
                user.PASSWORD = MSEncrypto.Encryption.Encrypt("");
            }
            else
            {
                user.PASSWORD = MSEncrypto.Encryption.Encrypt("Abc@123");
            }
            try
            {
                DataSet dataSet = InsertUserMaster(user.USERNAME, user.PASSWORD, user.FULLNAME, av.UserCode.ToString(), user.SECURITYQUESTION, user.SECURITYANSWER, Convert.ToBoolean(user.ISADUSER), Convert.ToBoolean(user.ISADMIN));
                ViewBag.SecurityMaster = GetSecurityQuestions();

                if (dataSet != null && dataSet.Tables[0].Rows.Count > 0)
                {
                    if (dataSet.Tables[0].Rows[0][0].ToString().ToUpper() == "SAVED")
                    {
                        TempData["Message"] = "success|User added successfully";
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        TempData["Message"] = "error|User name already exists!";
                        return View();
                    }
                }
                else
                {
                    TempData["Message"] = "success|Error occurred while creating user!";
                    return View();
                }
            }
            catch (Exception ex)
            {
                FormsAuthentication.LogException(ex, Request, DI.session, "UserMaster", "Create", DI.dBAccess);
                TempData["Message"] = "error|Error occurred while creating user!";
                return View();
            }
        }

        public IActionResult Edit(string Code)
        {
            try
            {
                ActiveUser av = FormsAuthentication.GetCurrentUser(DI.session);
                ViewBag.IsAdmin = Convert.ToInt32(av.UserCode);
                DataSet dataSet = SelectUserMaster(Code);
                List<UserMasterModel> userMaster = new List<UserMasterModel>();
                for (int i = 0; i < dataSet.Tables[0].Rows.Count; i++)
                {
                    userMaster.Add(new UserMasterModel
                    {

                        LOGINATTEMPTS = Convert.ToInt32(dataSet.Tables[0].Rows[i]["login_attempt"]),
                        FULLNAME = dataSet.Tables[0].Rows[i]["FullName"].ToString(),
                        USERNAME = dataSet.Tables[0].Rows[i]["UserName"].ToString(),
                        ISADUSER = Convert.ToBoolean(Convert.ToInt16(dataSet.Tables[0].Rows[i]["IsADUser"])),
                        ISADMIN = Convert.ToBoolean(Convert.ToInt16(dataSet.Tables[0].Rows[i]["IsAdmin"])),
                        SECURITYQUESTION = dataSet.Tables[0].Rows[i]["SecurityQuestion"].ToString(),
                        SECURITYANSWER = dataSet.Tables[0].Rows[i]["SecurityAnswer"].ToString(),
                        LOCKED = Convert.ToBoolean(Convert.ToInt16(dataSet.Tables[0].Rows[i]["Locked"])),
                        CODE = dataSet.Tables[0].Rows[i]["Code"].ToString()
                    });
                }

                ViewBag.SecurityMaster = GetSecurityQuestions();
                return View("Edit", userMaster);
            }
            catch (Exception ex)
            {
                FormsAuthentication.LogException(ex, Request, DI.session, "UserMaster", "Edit(string code)", DI.dBAccess);
                return View();
            }
        }
        [HttpPost]
        public IActionResult Edit([Bind] UserMasterModel user)
        {
            try
            {
                ActiveUser av = FormsAuthentication.GetCurrentUser(DI.session);
                DataSet dataSet = UpdateUserMaster(user.CODE, av.UserCode.ToString(), user.USERNAME, user.FULLNAME, Convert.ToBoolean(user.LOCKED), user.SECURITYQUESTION, user.SECURITYANSWER, Convert.ToBoolean(user.ISADUSER), Convert.ToBoolean(user.ISADMIN),
    user.LOGINATTEMPTS);
                TempData["Message"] = "success|User updated successfully";
            }
            catch (Exception ex)
            {
                FormsAuthentication.LogException(ex, Request, DI.session, "UserMaster", "Edit", DI.dBAccess);
                TempData["Message"] = "error|Error occurred while updating record";
            }
            return RedirectToAction("Index");
        }
        public IActionResult Delete(int Code)
        {
            try
            {
                ActiveUser av = FormsAuthentication.GetCurrentUser(DI.session);
                DataSet dataSet = UserDelete(Code,Convert.ToInt32(av.UserCode));
                TempData["Message"] = "success|User deleted successfully";
            }
            catch (Exception ex)
            {
                FormsAuthentication.LogException(ex, Request, DI.session, "UserMaster", "Delete", DI.dBAccess);
                TempData["Message"] = "error|Error occurred while deleting record";
            }
            return RedirectToAction("Index");
        }

        public List<SecurityQuestion> GetSecurityQuestions()
        {
            List<OracleParameter> commands = new List<OracleParameter>();
            commands.Add(new OracleParameter("v_cursor", OracleDbType.RefCursor, null, System.Data.ParameterDirection.Output));
            DataSet dataSet = DI.dBAccess.ExecuteDataSet_ADM("USP_BOB_ADM_PGETSECURITYQUESTIONS", commands);
            List<SecurityQuestion> securityQuestion = new List<SecurityQuestion>();
            for (int i = 0; i < dataSet.Tables[0].Rows.Count; i++)
            {
                securityQuestion.Add(new SecurityQuestion
                {
                    SECURITYQUESTION = dataSet.Tables[0].Rows[i]["Question"].ToString(),
                    Value = Convert.ToInt32(dataSet.Tables[0].Rows[i]["Code"].ToString())
                });
            }
            return securityQuestion;
        }

        public DataSet InsertUserMaster(string UserName, string Password, string FullName,
           string CreatedBy, string SeqQuestion, string SeqAnswer, bool IsADUser,
           bool IsAdminUser)
        {

            List<OracleParameter> commands = new List<OracleParameter>();

            commands.Add(new OracleParameter("iv_Code", OracleDbType.Int16, 1, System.Data.ParameterDirection.Input));
            commands.Add(new OracleParameter("v_UserName", OracleDbType.Varchar2, UserName, System.Data.ParameterDirection.Input));
            commands.Add(new OracleParameter("v_Password", OracleDbType.Varchar2, Password, System.Data.ParameterDirection.Input));
            commands.Add(new OracleParameter("v_FullName", OracleDbType.Varchar2, FullName, System.Data.ParameterDirection.Input));
            commands.Add(new OracleParameter("v_MobileNo", OracleDbType.Varchar2, "", System.Data.ParameterDirection.Input));
            commands.Add(new OracleParameter("v_Gender", OracleDbType.Int16, 1, System.Data.ParameterDirection.Input));
            commands.Add(new OracleParameter("v_DOB", OracleDbType.Varchar2, "", System.Data.ParameterDirection.Input));
            commands.Add(new OracleParameter("v_BloodGroup", OracleDbType.Varchar2, "", System.Data.ParameterDirection.Input));
            commands.Add(new OracleParameter("v_ImageName", OracleDbType.Varchar2, "", System.Data.ParameterDirection.Input));
            commands.Add(new OracleParameter("v_ExperienceLevel", OracleDbType.Int16, 1, System.Data.ParameterDirection.Input));
            commands.Add(new OracleParameter("v_IsADUser", OracleDbType.Int16, IsADUser ? 1 : 0, System.Data.ParameterDirection.Input));
            commands.Add(new OracleParameter("v_IsAdmin", OracleDbType.Int16, IsAdminUser ? 1 : 0, System.Data.ParameterDirection.Input));
            commands.Add(new OracleParameter("v_LoginAttempts", OracleDbType.Int16, 1, System.Data.ParameterDirection.Input));
            commands.Add(new OracleParameter("v_CreatedBy", OracleDbType.Int16, Convert.ToInt32(CreatedBy), System.Data.ParameterDirection.Input));
            commands.Add(new OracleParameter("v_SecurityQuestion", OracleDbType.Int16, Convert.ToInt32(SeqQuestion), System.Data.ParameterDirection.Input));
            commands.Add(new OracleParameter("v_SecurityAnswer", OracleDbType.Varchar2, SeqAnswer, System.Data.ParameterDirection.Input));
            commands.Add(new OracleParameter("v_isEdit", OracleDbType.Int16, 1, System.Data.ParameterDirection.Input));
            commands.Add(new OracleParameter("v_isDelete", OracleDbType.Int16, 1, System.Data.ParameterDirection.Input));
            commands.Add(new OracleParameter("v_cursor", OracleDbType.RefCursor, null, System.Data.ParameterDirection.Output));

            DataSet ds = DI.dBAccess.ExecuteDataSet_ADM("USP_BOB_ADM_usermaster_INSERT", commands);
            return ds;
        }

        public DataSet SelectUserMaster(string UserCode)
        {
            List<OracleParameter> commands = new List<OracleParameter>();

            commands.Add(new OracleParameter("v_Code", OracleDbType.Varchar2, UserCode, System.Data.ParameterDirection.Input));
            commands.Add(new OracleParameter("v_cursor", OracleDbType.RefCursor, null, System.Data.ParameterDirection.Output));

            DataSet ds = DI.dBAccess.ExecuteDataSet_ADM("USP_BOB_ADM_USERMASTER_SELECT", commands);
            return ds;
        }

        public DataSet UpdateUserMaster(string Code, string ModifiedBy, string UserName, string FullName, bool Locked,
          string SeqQuestion, string SeqAnswer, bool IsADUser, bool IsAdminUser,int LOGIN_ATTEMPT)
        {
            List<OracleParameter> commands = new List<OracleParameter>();

            commands.Add(new OracleParameter("v_Code", OracleDbType.Int16, Convert.ToInt32(Code), System.Data.ParameterDirection.Input));
            commands.Add(new OracleParameter("v_FullName", OracleDbType.Varchar2, FullName, System.Data.ParameterDirection.Input));
            commands.Add(new OracleParameter("v_ImageName", OracleDbType.Varchar2, "", System.Data.ParameterDirection.Input));
            commands.Add(new OracleParameter("v_IsADUser", OracleDbType.Int16, IsADUser ? 1 : 0, System.Data.ParameterDirection.Input));
            commands.Add(new OracleParameter("v_IsAdmin", OracleDbType.Int16, IsAdminUser ? 1 : 0, System.Data.ParameterDirection.Input));
            commands.Add(new OracleParameter("v_Locked", OracleDbType.Int16, Locked ? 1 : 0, System.Data.ParameterDirection.Input));
            commands.Add(new OracleParameter("v_LoginAttempts", OracleDbType.Int16, 1, System.Data.ParameterDirection.Input));
            commands.Add(new OracleParameter("v_LastModifiedBy", OracleDbType.Int16, Convert.ToInt32(ModifiedBy), System.Data.ParameterDirection.Input));
            commands.Add(new OracleParameter("v_SecurityQuestion", OracleDbType.Int16, Convert.ToInt32(SeqQuestion), System.Data.ParameterDirection.Input));
            commands.Add(new OracleParameter("v_SecurityAnswer", OracleDbType.Varchar2, SeqAnswer, System.Data.ParameterDirection.Input));
            commands.Add(new OracleParameter("v_isEdit", OracleDbType.Int16, 1, System.Data.ParameterDirection.Input));
            commands.Add(new OracleParameter("v_isDelete", OracleDbType.Int16, 1, System.Data.ParameterDirection.Input));
            commands.Add(new OracleParameter("v_Login_Attempt", OracleDbType.Int16, LOGIN_ATTEMPT, System.Data.ParameterDirection.Input));
            commands.Add(new OracleParameter("v_cursor", OracleDbType.RefCursor, null, System.Data.ParameterDirection.Output));

            DataSet ds = DI.dBAccess.ExecuteDataSet_ADM("USP_BOB_ADM_USERMASTER_UPDATE", commands);
            return ds;
        }

        public DataSet UserDelete(int Code,int deletedBy)
        {
            List<OracleParameter> commands = new List<OracleParameter>();
            commands.Add(new OracleParameter("v_Code", OracleDbType.Int16, Convert.ToInt32(Code), System.Data.ParameterDirection.Input));
            commands.Add(new OracleParameter("V_DELETED_BY", OracleDbType.Int32, Convert.ToInt32(Code), System.Data.ParameterDirection.Input));
            commands.Add(new OracleParameter("v_cursor", OracleDbType.RefCursor, null, System.Data.ParameterDirection.Output));
            DataSet ds = DI.dBAccess.ExecuteDataSet_ADM("USP_BOB_ADM_USERMASTER_DELETE", commands);
            return ds;
        }
    }
}
