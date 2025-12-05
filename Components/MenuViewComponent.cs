using MasterApplication.Areas.Admin.Models;
using MasterApplication.Services;
using Microsoft.AspNetCore.Mvc;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace MasterApplication.Areas.Admin.Components
{
    public class MenuViewComponent : ViewComponent
    {
        readonly DependancyInjection DI;
        //readonly FormsAuthentication formsAuthentication;

        public MenuViewComponent(DependancyInjection _dependancyInjection)
        {
            DI = _dependancyInjection;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            List<MenuModel> menu = new List<MenuModel>();
            ActiveUser currentUser = FormsAuthentication.GetCurrentUser(DI.session);
            if (currentUser == null)
            {
                ViewBag.SessionMessage = "Your session has expired,Kindly login again!";
                MainMenuModel mainMenu1 = new MainMenuModel();
                mainMenu1.menuModel = menu;
                mainMenu1.LoginURL = DI.myAppSettings.LoginURL;
                return View("_app-sidebar", mainMenu1);
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
                    AreaName = dataSet.Tables[0].Rows[i]["AreaName"].ToString()
                });
            }
            MainMenuModel mainMenu = new MainMenuModel();
            mainMenu.menuModel = menu;
            mainMenu.LoginURL = DI.myAppSettings.LoginURL;
            return View("_app-sidebar", mainMenu);
        }
    }
}
