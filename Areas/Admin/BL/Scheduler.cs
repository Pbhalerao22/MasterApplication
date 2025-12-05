using MasterApplication.DAL;
using Oracle.ManagedDataAccess.Client;
using System.Collections.Generic;
using System.Data;
using System;

namespace MasterApplication.Areas.Admin.BL
{
    public class Scheduler
    {
        public static DataSet SelectSchedular(string UserCode, DBAccess _dBAccess)
        {
            List<OracleParameter> commands = new List<OracleParameter>();

            commands.Add(new OracleParameter("P_Code", OracleDbType.Varchar2, UserCode, System.Data.ParameterDirection.Input));
            commands.Add(new OracleParameter("O_cursor", OracleDbType.RefCursor, null, System.Data.ParameterDirection.Output));

            DataSet ds = _dBAccess.ExecuteDataSet("USP_EMPANEL_IRCTC_SCHEDULER_SELECT", commands);
            return ds;
        }

        public static DataSet UpdateSchedular(string Code, string strProjectName, string strProcesstype, string Exection_Date, string Execution_Time, string Email_Ids, string PROCESS_STATUS, DBAccess _dBAccess)
        {
            List<OracleParameter> commands = new List<OracleParameter>();

            commands.Add(new OracleParameter("P_Code", OracleDbType.Varchar2, Code, System.Data.ParameterDirection.Input));
            commands.Add(new OracleParameter("p_project_name", OracleDbType.Varchar2, strProjectName, System.Data.ParameterDirection.Input));
            commands.Add(new OracleParameter("P_Process_type", OracleDbType.Varchar2, strProcesstype, System.Data.ParameterDirection.Input));
            commands.Add(new OracleParameter("P_Exection_Date", OracleDbType.Varchar2, Exection_Date, System.Data.ParameterDirection.Input));
            commands.Add(new OracleParameter("P_Execution_Time", OracleDbType.Varchar2, Execution_Time, System.Data.ParameterDirection.Input));
            commands.Add(new OracleParameter("P_Email_Ids", OracleDbType.Varchar2, Email_Ids, System.Data.ParameterDirection.Input));
            commands.Add(new OracleParameter("P_PROCESS_STATUS", OracleDbType.Varchar2, PROCESS_STATUS, System.Data.ParameterDirection.Input));
            commands.Add(new OracleParameter("O_cursor", OracleDbType.RefCursor, null, System.Data.ParameterDirection.Output));

            DataSet ds = _dBAccess.ExecuteDataSet("usp_bob_scheduler_update", commands);
            return ds;
        }

        public static DataSet InsertSchedular(string strProjectName, string strProcessType, string Execution_Date, string Execution_Time, string strProcessStatus, Int32 UserCode, DBAccess _dBAccess)
        {
            List<OracleParameter> commands = new List<OracleParameter>();
            //   commands.Add(new OracleParameter("iv_Code", OracleDbType.Varchar2, Code, ParameterDirection.Input));
            commands.Add(new OracleParameter("P_Project_Name", OracleDbType.Varchar2, strProjectName, ParameterDirection.Input));
            commands.Add(new OracleParameter("P_PROCESS_TYPE", OracleDbType.Varchar2, strProcessType, ParameterDirection.Input));
            commands.Add(new OracleParameter("P_EXECUTION_DATE", OracleDbType.Varchar2, Execution_Date, ParameterDirection.Input));
            commands.Add(new OracleParameter("P_EXECUTION_TIME", OracleDbType.Varchar2, Execution_Time, ParameterDirection.Input));
            commands.Add(new OracleParameter("P_PROCESS_STATUS", OracleDbType.Varchar2, strProcessStatus, ParameterDirection.Input));
            commands.Add(new OracleParameter("P_CREATEDBY", OracleDbType.Varchar2, UserCode, ParameterDirection.Input));
            commands.Add(new OracleParameter("O_CURSOR", OracleDbType.RefCursor, null, ParameterDirection.Output));

            DataSet ds = _dBAccess.ExecuteDataSet("usp_bob_scheduler_insert", commands);
            return ds;
        }
        public static DataSet EditSelectSchedular(string UserCode, DBAccess _dBAccess)
        {
            List<OracleParameter> commands = new List<OracleParameter>();

            commands.Add(new OracleParameter("P_CODE", OracleDbType.Varchar2, UserCode, System.Data.ParameterDirection.Input));
            commands.Add(new OracleParameter("O_CURSOR", OracleDbType.RefCursor, null, System.Data.ParameterDirection.Output));

            DataSet ds = _dBAccess.ExecuteDataSet("USP_BOB_FETCH_SCHEDULER_EDIT", commands);
            return ds;
        }
    }
}
