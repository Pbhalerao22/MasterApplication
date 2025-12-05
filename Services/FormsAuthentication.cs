using MasterApplication.DAL;
using Microsoft.AspNetCore.Http;
using Oracle.ManagedDataAccess.Client;
using Renci.SshNet;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace MasterApplication.Services
{
    public class FormsAuthentication
    {
        public FormsAuthentication(DBAccess dB)
        {
            //dBAccess = dB;

        }
        public static ActiveUser ForceLoginInfo(HttpContext httpContext, ISession session, int UserCode, string UserName, string Password, string LoginDateTime, string ExpiryDateTime,
           bool IsADuser, bool IsLocked, string strImageName, string strLoginTime, bool IsAdmin, bool isEdit, bool isDelete, DBAccess dBAccess)
        {
            ActiveUser activeUser = new ActiveUser();
            activeUser.UserCode = UserCode;
            activeUser.UserName = UserName;
            activeUser.Password = Password;
            activeUser.IsADuser = IsADuser;
            activeUser.IsLocked = IsLocked;
            activeUser.IsAdmin = IsAdmin;
            activeUser.isEdit = isEdit;
            activeUser.isDelete = isDelete;
            activeUser.loginSession = System.Guid.NewGuid().ToString();
            session.Clear();
            httpContext.Session.Clear();

            if (httpContext.Request.Cookies[".AspNetCore.Session"] != null)
            {
                httpContext.Response.Cookies.Delete(".AspNetCore.Session");
            }

            return activeUser;

        }
        public static void SaveLogin(ActiveUser activeUser, ISession session, DBAccess dBAccess)
        {
            session.SetObjectAsJson("LoginInfo", activeUser);

            DataTable dtResult = UpdateUserActivityLog(session, "Login", true, false, false, dBAccess);
            activeUser.LoginTime = dtResult.Rows[0][0].ToString();

        }

        public static void NormalLoginUpdate(HttpContext httpContext, ISession session, int UserCode, string UserName, string Password, string LoginDateTime, string ExpiryDateTime,
            bool IsADuser, bool IsLocked, string strImageName, string strLoginTime, bool IsAdmin, bool isEdit, bool isDelete, DBAccess dBAccess)
        {
            ActiveUser activeUser = new ActiveUser();
            activeUser.UserCode = UserCode;
            activeUser.UserName = UserName;
            activeUser.Password = Password;
            activeUser.IsADuser = IsADuser;
            activeUser.IsLocked = IsLocked;
            activeUser.IsAdmin = IsAdmin;
            activeUser.isEdit = isEdit;
            activeUser.isDelete = isDelete;
            activeUser.loginSession = System.Guid.NewGuid().ToString();
            session.Clear();
            httpContext.Session.Clear();

            if (httpContext.Request.Cookies[".AspNetCore.Session"] != null)
            {
                httpContext.Response.Cookies.Delete(".AspNetCore.Session");
            }
            session.SetObjectAsJson("LoginInfo", activeUser);

            DataTable dtResult = UpdateUserActivityLog(session, "Login", true, false, false, dBAccess);
            activeUser.LoginTime = dtResult.Rows[0][0].ToString();

        }

        public static DataTable UpdateUserActivityLog(ISession session,
           string CallerMenu, bool IsLogin, bool IsLogOut, bool IsNormalLogOut, DBAccess dBAccess)
        {
            try
            {
                int isLogin = IsLogin == true ? 1 : 0;
                int isLogout = IsLogOut == true ? 1 : 0;
                int isNormalLogOut = IsNormalLogOut == true ? 1 : 0;
                ActiveUser av = GetCurrentUser(session);


                List<OracleParameter> commands = new List<OracleParameter>();

                commands.Add(new OracleParameter("p_UserCode", OracleDbType.Varchar2, av.UserCode, System.Data.ParameterDirection.Input));
                commands.Add(new OracleParameter("p_AspSessionID", OracleDbType.Varchar2, session.Id, System.Data.ParameterDirection.Input));
                commands.Add(new OracleParameter("p_FormName", OracleDbType.Varchar2, CallerMenu, System.Data.ParameterDirection.Input));
                commands.Add(new OracleParameter("p_IsLogin", OracleDbType.Int32, isLogin, System.Data.ParameterDirection.Input));
                commands.Add(new OracleParameter("p_IsLogOut", OracleDbType.Int32, isLogout, System.Data.ParameterDirection.Input));
                commands.Add(new OracleParameter("p_IsNormalLogOut", OracleDbType.Int32, isNormalLogOut, System.Data.ParameterDirection.Input));
                commands.Add(new OracleParameter("p_LoginSession", OracleDbType.Varchar2, av.loginSession, System.Data.ParameterDirection.Input));
                commands.Add(new OracleParameter("RESULT1", OracleDbType.RefCursor, null, System.Data.ParameterDirection.Output));

                //DataSet dtResult = dBAccess.ExecuteDataSet("USP_BOB_ADM_USERACTIVITYLOG_UPDATE", commands);
                DataSet dtResult = dBAccess.ExecuteDataSet_ADM("USP_BOB_ADM_USERACTIVITYLOG_UPDATE", commands);
                return dtResult.Tables[0];

            }
            catch (Exception ex)
            {

                throw;
            }

        }

        public static ActiveUser GetCurrentUser(ISession Session)
        {
            return (ActiveUser)SessionHelper.GetObjectFromJson<ActiveUser>(Session, "LoginInfo");
        }

        public static void LogException(Exception ex, HttpRequest request, ISession session, string FormName, string MethodName, DBAccess dBAccess)
        {
            try
            {
                string a = request.Path;
                // Get UserHostAddress property.
                string MachineName = Environment.MachineName;
                string url = request.Path;
                // ht tp://localhost:1302/TESTERS/Default6.aspx
                string path = request.Path;
                // /TESTERS/Default6.aspx
                string host = request.Host.Value;
                ActiveUser av = GetCurrentUser(session);
                List<OracleParameter> commands = new List<OracleParameter>();

                commands.Add(new OracleParameter("p_UserCode", OracleDbType.Varchar2, av.UserCode, System.Data.ParameterDirection.Input));
                commands.Add(new OracleParameter("p_MachineName", OracleDbType.Varchar2, (MachineName.Trim().Length == 0 ? host : MachineName), System.Data.ParameterDirection.Input));
                commands.Add(new OracleParameter("p_Url", OracleDbType.Varchar2, url, System.Data.ParameterDirection.Input));
                commands.Add(new OracleParameter("p_FormName", OracleDbType.Varchar2, FormName, System.Data.ParameterDirection.Input));
                commands.Add(new OracleParameter("p_MethodName", OracleDbType.Varchar2, MethodName, System.Data.ParameterDirection.Input));
                commands.Add(new OracleParameter("p_ExceptionDetails", OracleDbType.Varchar2, ("Exception Message : " + ex.Message + " -:-> Inner Exception :" + ex.ToString()), System.Data.ParameterDirection.Input));
                commands.Add(new OracleParameter("ResulSet", OracleDbType.RefCursor, null, System.Data.ParameterDirection.Output));


                DataSet ds = dBAccess.ExecuteDataSet("EXCEPTIONLOG_INSERT", commands);

            }
            catch (Exception ef)
            {
                //if (!Directory.Exists(HttpContext.Current.Server.MapPath("~/Logs/")))
                //{
                //    Directory.CreateDirectory(HttpContext.Current.Server.MapPath("~/Logs/"));
                //}
                //using (StreamWriter _LogException = new StreamWriter(HttpContext.Current.Server.MapPath("~/Logs/" + DateTime.Now.Day.ToString() + "_" + DateTime.Now.Month.ToString() + "_" + DateTime.Now.Year.ToString() + ".txt"), true))
                //{
                //    _LogException.WriteLine(DateTime.Now.ToString() + " ==> " + ef.Message + ef.StackTrace); // Write the file.
                //}
            }


        }

        public static bool isSessionValid(ISession session)
        {
            if (session != null)
            {
                return true;
            }
            return false;
        }

     
    }
}
