using MasterApplication.Areas.Admin.Models;
using MasterApplication.Services;
using Microsoft.AspNetCore.Mvc;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace MasterApplication.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class HomeController : Controller
    {
        readonly DependancyInjection DI;
        public HomeController(DependancyInjection _dependancyInjection)
        {
            
            DI = _dependancyInjection;
            
        }
        
        public IActionResult Index()
        {
            //GetMenuList();
            
            return View();
        }

        public ActionResult GetMenuList()
        {
            List<MenuModel> menu = new List<MenuModel>();
            ActiveUser currentUser = FormsAuthentication.GetCurrentUser(DI.session);
            if (currentUser == null)
            {
                ViewBag.SessionMessage = "Your session has expired,Kindly login again!";
                //return View("_SideBar", menu);
            }

            List<OracleParameter> commands = new List<OracleParameter>();

            commands.Add(new OracleParameter("p_UserCode", OracleDbType.Varchar2, currentUser.UserCode, System.Data.ParameterDirection.Input));
            commands.Add(new OracleParameter("cur", OracleDbType.RefCursor, null, System.Data.ParameterDirection.Output));

            DataSet dataSet = DI.dBAccess.ExecuteDataSet_ADM("USP_BOB_ADM_ALLOWEDMENU", commands);

            for (int i = 0; i < dataSet.Tables[0].Rows.Count; i++)
            {
                menu.Add(new MenuModel
                {
                    MENUCODE = Convert.ToInt32(dataSet.Tables[0].Rows[i]["MENUCODE"].ToString().Length != 0 ? dataSet.Tables[0].Rows[i]["MENUCODE"] : 0),
                    MENUNAME = dataSet.Tables[0].Rows[i]["MENUNAME"].ToString(),
                    MENUDESC = dataSet.Tables[0].Rows[i]["MENUDESC"].ToString(),
                    PARENTID = Convert.ToInt32(dataSet.Tables[0].Rows[i]["PARENTID"].ToString().Length != 0 ? dataSet.Tables[0].Rows[i]["PARENTID"] : 0),
                    MENUURL = dataSet.Tables[0].Rows[i]["MENUURL"].ToString(),
                    MENUICON = dataSet.Tables[0].Rows[i]["MENUICON"].ToString(),
                    ISTOPMENU = Convert.ToInt32(dataSet.Tables[0].Rows[i]["ISTOPMENU"].ToString().Length != 0 ? dataSet.Tables[0].Rows[i]["ISTOPMENU"] : 0),
                    MENUSRNO = Convert.ToInt32(dataSet.Tables[0].Rows[i]["MENUSRNO"].ToString().Length != 0 ? dataSet.Tables[0].Rows[i]["MENUSRNO"] : 0),
                    CONTROLLERNAME = dataSet.Tables[0].Rows[i]["CONTROLLERNAME"].ToString(),
                    ActionName = dataSet.Tables[0].Rows[i]["ActionName"].ToString(),
                    AreaName= dataSet.Tables[0].Rows[i]["AreaName"].ToString()

                });
            }
            //DI.session.SetObjectAsJson("UserMenuList", menu);
            ViewBag.UserMenuList = menu;
            return View("_app-sidebar", menu);


        }
        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult ErrorPage()
        {
            return RedirectToAction("Error", "Home");
        }
        public IActionResult Error()
        {
            return View();
            //return View("Error");
            //return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        [HttpPost]
        public ActionResult ChangeRolePreference(string RoleCode, string DefaultRole)
        {
            try
            {
                ActiveUser av = FormsAuthentication.GetCurrentUser(DI.session);

                BL.Home.UpdateRolePreference(RoleCode, av.UserCode.ToString(), DI.dBAccess);

                TempData["Message"] = "success|Role updated successfully";
                return Json(new { Message = "Success" });
            }
            catch (Exception ex)
            {
                FormsAuthentication.LogException(ex, Request, DI.session, "UserRoleMapping", "UpdateUserRoleMapping", DI.dBAccess);
                TempData["Message"] = "error|Error occurred while update role!";
                return Json(new { Message = "Error" });
            }

            
        }
        public IActionResult GetMenu()
        {
            return View("Index");
        }
    }
}
