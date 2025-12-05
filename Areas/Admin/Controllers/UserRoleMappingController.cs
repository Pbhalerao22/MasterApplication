using MasterApplication.Areas.Admin.Models;
using MasterApplication.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace MasterApplication.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class UserRoleMappingController : Controller
    {
        readonly DependancyInjection DI;
        public UserRoleMappingController(DependancyInjection _DI)
        {
            DI = _DI;
        }
        public IActionResult Index()
        {
            UserRoleModel userRoleModel = new UserRoleModel();
            List<UserList> UserObj = new List<UserList>();
            List<UserRoleList> UserRoleObj = new List<UserRoleList>();
            Dictionary<string, string> keyValuePairs = BL.UserRoleMapping.GetListOfUsers(DI.dBAccess);
            if (keyValuePairs.Count > 0)
            {
                foreach (var item in keyValuePairs)
                {
                    UserList userList = new UserList();
                    userList.UserCode = Convert.ToInt32(item.Key);
                    userList.UserName = item.Value;
                    UserObj.Add(userList);
                }
                userRoleModel.userLists = UserObj;

            }
            DataSet dataSet = BL.UserRoleMapping.GetUserRoleDetails(0, DI.dBAccess);
            if (dataSet != null && dataSet.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < dataSet.Tables[0].Rows.Count; i++)
                {
                    UserRoleList userRoleList = new UserRoleList();
                    userRoleList.RoleCode = Convert.ToInt32(dataSet.Tables[0].Rows[i]["Code"]);
                    userRoleList.RoleName = dataSet.Tables[0].Rows[i]["RoleName"].ToString();
                    userRoleList.IsAssigned = Convert.ToBoolean(dataSet.Tables[0].Rows[i]["IsAssigned"]);
                    userRoleList.DefaultRole = Convert.ToBoolean(dataSet.Tables[0].Rows[i]["DEFAULTROLE"]);
                    UserRoleObj.Add(userRoleList);
                }
                userRoleModel.userRoleLists = UserRoleObj;
            }
            return View(userRoleModel);
        }
        public IActionResult GetUserRoleDetails(int UserCode)
        {

            UserRoleModel userRoleModel = new UserRoleModel();
            userRoleModel.UserCode = UserCode;
            List<UserList> UserObj = new List<UserList>();
            List<UserRoleList> UserRoleObj = new List<UserRoleList>();
            Dictionary<string, string> keyValuePairs = BL.UserRoleMapping.GetListOfUsers(DI.dBAccess);
            if (keyValuePairs.Count > 0)
            {
                foreach (var item in keyValuePairs)
                {
                    UserList userList = new UserList();
                    userList.UserCode = Convert.ToInt32(item.Key);
                    userList.UserName = item.Value;
                    UserObj.Add(userList);
                }
                userRoleModel.userLists = UserObj;

            }
            DataSet dataSet = BL.UserRoleMapping.GetUserRoleDetails(UserCode, DI.dBAccess);
            if (dataSet != null && dataSet.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < dataSet.Tables[0].Rows.Count; i++)
                {
                    UserRoleList userRoleList = new UserRoleList();
                    userRoleList.RoleCode = Convert.ToInt32(dataSet.Tables[0].Rows[i]["Code"]);
                    userRoleList.RoleName = dataSet.Tables[0].Rows[i]["RoleName"].ToString();
                    userRoleList.IsAssigned = Convert.ToBoolean(dataSet.Tables[0].Rows[i]["IsAssigned"]);
                    userRoleList.DefaultRole = Convert.ToBoolean(dataSet.Tables[0].Rows[i]["DEFAULTROLE"]);
                    UserRoleObj.Add(userRoleList);
                }
                userRoleModel.userRoleLists = UserRoleObj;
            }
            return View("Index", userRoleModel);
        }
        [HttpPost]
        public JsonResult UpdateUserRoleMapping([FromBody] List<UserRoleList> UserRoleList)
        {
            try
            {
                ActiveUser av = FormsAuthentication.GetCurrentUser(DI.session);
                for (int i = 0; i < UserRoleList.Count; i++)
                {
                    BL.UserRoleMapping.UpdateUserRoleMapping(UserRoleList[i].UserCode.ToString(), UserRoleList[i].RoleName, UserRoleList[i].IsAssigned, UserRoleList[i].DefaultRole, av.UserCode, DI.dBAccess);
                }
                TempData["Message"] = "success|Role updated successfully";
                return Json(new { Message = "Success" });
            }
            catch (Exception ex)
            {
                FormsAuthentication.LogException(ex, Request, DI.session, "UserRoleMapping", "UpdateUserRoleMapping", DI.dBAccess);
                TempData["Message"] = "error|Error occurred while update role!";
                return Json(new { Message = "Error" });
            }
            //return View();
        }
    }
}
