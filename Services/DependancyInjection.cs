using MasterApplication.Areas.Admin.Models;
using MasterApplication.DAL;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace MasterApplication.Services
{
    public class DependancyInjection
    {
        public readonly DBAccess dBAccess;
        public readonly MyAppSettings myAppSettings;
        public readonly FormsAuthentication forms;

        public readonly CommonClass commonClass;
        //readonly FormsAuthentication formsAuthentication;
        public readonly IHttpContextAccessor _httpContextAccessor;
        public IWebHostEnvironment _hostEnvironment;

        public ISession session => _httpContextAccessor.HttpContext.Session;

        
        public DependancyInjection()
        {

        }
        public DependancyInjection(DBAccess dB, FormsAuthentication _forms, IHttpContextAccessor httpContextAccessor,
            IOptions<MyAppSettings> _myAppSettings, IWebHostEnvironment environment, CommonClass _CommonClass)
        {
            dBAccess = dB;
            _httpContextAccessor = httpContextAccessor;
            forms = _forms;
            myAppSettings = _myAppSettings.Value;
            _hostEnvironment = environment;
            commonClass = _CommonClass;

            
        }

        public List<MenuModel> GetMenuList(ISession session)
        {
            List<MenuModel> menu = new List<MenuModel>();
            ActiveUser currentUser = FormsAuthentication.GetCurrentUser(session);
            if (currentUser == null)
            {

                return menu;
            }

            List<OracleParameter> commands = new List<OracleParameter>();

            commands.Add(new OracleParameter("p_UserCode", OracleDbType.Varchar2, currentUser.UserCode, System.Data.ParameterDirection.Input));
            commands.Add(new OracleParameter("cur", OracleDbType.RefCursor, null, System.Data.ParameterDirection.Output));

            DataSet dataSet = dBAccess.ExecuteDataSet_ADM("USP_BOB_ADM_ALLOWEDMENU", commands);

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
            //DI.session.SetObjectAsJson("UserMenuList", menu);
            return menu;



        }
    }
}
