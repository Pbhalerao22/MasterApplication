using MasterApplication.Areas.Admin.Models;
using MasterApplication.DAL;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace MasterApplication.Areas.Admin.BL
{
    public class Role
    {
        public static List<SelectRole> GetSelectRoles(DBAccess _dbAccess)
        {
            List<OracleParameter> commands = new List<OracleParameter>();
            commands.Add(new OracleParameter("v_cursor", OracleDbType.RefCursor, null, System.Data.ParameterDirection.Output));
            DataSet dataSet = _dbAccess.ExecuteDataSet_ADM("USP_BOB_ADM_ROLEMASTER", commands);
            List<SelectRole> selectRoles = new List<SelectRole>();
            for (int i = 0; i < dataSet.Tables[0].Rows.Count; i++)
            {
                selectRoles.Add(new SelectRole
                {
                    RoleName = dataSet.Tables[0].Rows[i]["RoleName"].ToString(),
                    Code = Convert.ToInt32(dataSet.Tables[0].Rows[i]["Code"].ToString())
                });
            }
            return selectRoles;
        }
        public static DataSet GetRoleMenuMappingMaster(string SearchValue, string SearchColumn, string OrderByCol,
          string OrderByType, int PageNo, int NoOfRows, string RoleCode, DBAccess _dBAccess)
        {

            List<OracleParameter> commands = new List<OracleParameter>();
            //string RoleCode = "1";
            RoleCode = ((RoleCode == "" || RoleCode == null) ? "1" : RoleCode);

            commands.Add(new OracleParameter("p_SearchColumn", OracleDbType.Varchar2, SearchColumn, System.Data.ParameterDirection.Input));
            commands.Add(new OracleParameter("p_SearchValue", OracleDbType.Varchar2, SearchValue, System.Data.ParameterDirection.Input));
            commands.Add(new OracleParameter("p_OrderByCol", OracleDbType.Varchar2, OrderByCol, System.Data.ParameterDirection.Input));
            commands.Add(new OracleParameter("p_OrderByType", OracleDbType.Varchar2, OrderByType, System.Data.ParameterDirection.Input));
            commands.Add(new OracleParameter("p_PageNo", OracleDbType.Int32, PageNo, System.Data.ParameterDirection.Input));
            commands.Add(new OracleParameter("p_NoOfRows", OracleDbType.Int32, NoOfRows, System.Data.ParameterDirection.Input));
            commands.Add(new OracleParameter("p_RoleCode", OracleDbType.Varchar2, RoleCode, System.Data.ParameterDirection.Input));
            commands.Add(new OracleParameter("v_CursorToalRecords", OracleDbType.RefCursor, null, System.Data.ParameterDirection.Output));
            commands.Add(new OracleParameter("v_CursorFetchRecords", OracleDbType.RefCursor, null, System.Data.ParameterDirection.Output));

            DataSet ds = _dBAccess.ExecuteDataSet("BYT_USP_ADM_GETROLEMAPPING", commands);
            return ds;
        }

        public static DataSet GetRoleMenuMapping(string RoleCode, DBAccess _dbAccess)
        {
            List<OracleParameter> commands = new List<OracleParameter>();

            commands.Add(new OracleParameter("v_RoleCode", OracleDbType.Int16, Convert.ToInt32(RoleCode), System.Data.ParameterDirection.Input));
            commands.Add(new OracleParameter("v_cursor", OracleDbType.RefCursor, null, System.Data.ParameterDirection.Output));

            DataSet ds = _dbAccess.ExecuteDataSet_ADM("USP_BOB_ADM_GETROLEMENUMAPPINGMASTER", commands);
            return ds;
        }

        public static DataSet InsertRoleMaster(string RoleCode, string MenuName, bool IsAssigned, string UserCode, DBAccess _dbAccess)
        {

            List<OracleParameter> commands = new List<OracleParameter>();

            commands.Add(new OracleParameter("v_RoleCode", OracleDbType.Int16, RoleCode, System.Data.ParameterDirection.Input));
            commands.Add(new OracleParameter("v_MenuName", OracleDbType.Varchar2, MenuName, System.Data.ParameterDirection.Input));
            commands.Add(new OracleParameter("v_IsAssigned", OracleDbType.Int16, IsAssigned ? 1 : 0, System.Data.ParameterDirection.Input));
            commands.Add(new OracleParameter("v_UserCode", OracleDbType.Int16, UserCode, System.Data.ParameterDirection.Input));
            commands.Add(new OracleParameter("v_cursor", OracleDbType.RefCursor, null, System.Data.ParameterDirection.Output));

            DataSet ds = _dbAccess.ExecuteDataSet_ADM("USP_BOB_ADM_ROLEMENUMASTER_UPDATE", commands);
            return ds;
        }
    }
}
