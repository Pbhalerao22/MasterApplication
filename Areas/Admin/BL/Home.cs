using MasterApplication.DAL;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace MasterApplication.Areas.Admin.BL
{
    public class Home
    {
        public static DataSet GetDashBoardData(DBAccess _dbAccess)
        {

            List<OracleParameter> commands = new List<OracleParameter>();

            commands.Add(new OracleParameter("P_DataSetTemplateCount", OracleDbType.RefCursor, null, System.Data.ParameterDirection.Output));
            commands.Add(new OracleParameter("P_DataSetRecordCount", OracleDbType.RefCursor, null, System.Data.ParameterDirection.Output));
            DataSet ds = _dbAccess.ExecuteDataSet("USP_BYT_GET_DashBoardData", commands);

            return ds;

        }

        public static DataSet UpdateRolePreference(string RoleCode, string UserCode, DBAccess _dbAccess)
        {

            List<OracleParameter> commands = new List<OracleParameter>();

            commands.Add(new OracleParameter("P_USERCODE", OracleDbType.Int32, UserCode, System.Data.ParameterDirection.Input));
            commands.Add(new OracleParameter("P_ROLECODE", OracleDbType.Int32, RoleCode, System.Data.ParameterDirection.Input));
            commands.Add(new OracleParameter("P_RESULTSET", OracleDbType.RefCursor, null, System.Data.ParameterDirection.Output));
            DataSet ds = _dbAccess.ExecuteDataSet_ADM("usp_admin_UpdateRolePreference", commands);

            return ds;

        }
    }
}
