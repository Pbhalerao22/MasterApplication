using MasterApplication.DAL;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace MasterApplication.Areas.Admin.BL
{
    public class UserRoleMapping
    {
        public static Dictionary<string, string> GetListOfUsers(DBAccess _dbAccess)
        {
            Dictionary<string, string> ListOfUser = new Dictionary<string, string>();
            List<OracleParameter> commands = new List<OracleParameter>();

            commands.Add(new OracleParameter("v_cursor", OracleDbType.RefCursor, null, System.Data.ParameterDirection.Output));
            DataSet ds = _dbAccess.ExecuteDataSet_ADM("USP_BOB_ADM_GETUSERMASTERFORMAPPING", commands);
            if (ds != null && ds.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    ListOfUser.Add(ds.Tables[0].Rows[i]["Code"].ToString(),
                                   ds.Tables[0].Rows[i]["UserName"].ToString());
                }
            }
            return ListOfUser;
        }

        public static DataSet GetUserRoleDetails(int UserCode, DBAccess _dbAccess)
        {
            List<OracleParameter> commands = new List<OracleParameter>();
            commands.Add(new OracleParameter("v_UserCode", OracleDbType.Int16, UserCode, System.Data.ParameterDirection.Input));
            commands.Add(new OracleParameter("v_cursor", OracleDbType.RefCursor, null, System.Data.ParameterDirection.Output));
            DataSet ds = _dbAccess.ExecuteDataSet_ADM("USP_BOB_ADM_GETUSERROLEMAPPINGMASTER", commands);
            return ds;
        }

        public static void UpdateUserRoleMapping(string strUserCode, string strRoleName,
                 bool IsChecked, bool DefaultRole, int LoginCode, DBAccess _dBAccess)
        {
            try
            {
                List<OracleParameter> commands = new List<OracleParameter>();
                commands.Add(new OracleParameter("p_UserCode", OracleDbType.Int16, Convert.ToInt32(strUserCode), System.Data.ParameterDirection.Input));
                commands.Add(new OracleParameter("p_RoleName", OracleDbType.Varchar2, strRoleName, System.Data.ParameterDirection.Input));
                commands.Add(new OracleParameter("p_IsAssigned", OracleDbType.Int16, IsChecked == true ? 1 : 0, System.Data.ParameterDirection.Input));
                commands.Add(new OracleParameter("p_DefaultRole", OracleDbType.Int16, DefaultRole == true ? 1 : 0, System.Data.ParameterDirection.Input));
                commands.Add(new OracleParameter("p_LoginCode", OracleDbType.Int16, Convert.ToInt32(LoginCode), System.Data.ParameterDirection.Input));
                commands.Add(new OracleParameter("p_cursor", OracleDbType.RefCursor, null, System.Data.ParameterDirection.Output));

                DataSet ds = _dBAccess.ExecuteDataSet_ADM("USP_BOB_ADM_USERROLEMASTER_UPDATE", commands);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
