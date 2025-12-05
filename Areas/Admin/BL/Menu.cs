using System;
using System.Collections.Generic;
using System.Linq;
using MasterApplication.Areas.Admin.Models;
using System.Threading.Tasks;
using Oracle.ManagedDataAccess.Client;
using System.Data;
using MasterApplication.DAL;

namespace MasterApplication.Areas.Admin.BL
{
    public class Menu
    {
        public static List<ParentMenus> GetParentMenus(DBAccess _dbAccess)
        {
            List<OracleParameter> commands = new List<OracleParameter>();
            commands.Add(new OracleParameter("v_cursor", OracleDbType.RefCursor, null, System.Data.ParameterDirection.Output));
            DataSet dataSet = _dbAccess.ExecuteDataSet_ADM("USP_BOB_ADM_PGETPARENTMENU", commands);
            List<ParentMenus> ParentMenu = new List<ParentMenus>();
            for (int i = 0; i < dataSet.Tables[0].Rows.Count; i++)
            {
                ParentMenu.Add(new ParentMenus
                {
                    MenuName = dataSet.Tables[0].Rows[i]["MenuName"].ToString(),
                    Code = Convert.ToInt32(dataSet.Tables[0].Rows[i]["Code"].ToString())
                });
            }
            return ParentMenu;
        }
        public static DataSet InsertMenuMaster(string MenuName, string MenuDesc, string ParentId,
            string MenuUrl, string MenuIcon, string MenuSrno,
          string CreatedBy, string ControllerName, string ActionName, string AreaName, DBAccess _dbAccess)
        {

            List<OracleParameter> commands = new List<OracleParameter>();

            commands.Add(new OracleParameter("iv_Code", OracleDbType.Int16, 1, System.Data.ParameterDirection.Input));
            commands.Add(new OracleParameter("v_MenuName", OracleDbType.Varchar2, MenuName, System.Data.ParameterDirection.Input));
            commands.Add(new OracleParameter("v_MenuDesc", OracleDbType.Varchar2, MenuDesc, System.Data.ParameterDirection.Input));
            commands.Add(new OracleParameter("iv_ParentId", OracleDbType.Int16, ParentId, System.Data.ParameterDirection.Input));
            commands.Add(new OracleParameter("v_MenuUrl", OracleDbType.Varchar2, MenuUrl, System.Data.ParameterDirection.Input));
            commands.Add(new OracleParameter("v_MenuIcon", OracleDbType.Varchar2, MenuIcon, System.Data.ParameterDirection.Input));
            commands.Add(new OracleParameter("v_MenuSrNo", OracleDbType.Int16, MenuSrno, System.Data.ParameterDirection.Input));
            commands.Add(new OracleParameter("v_CreatedBy", OracleDbType.Int16, CreatedBy, System.Data.ParameterDirection.Input));

            commands.Add(new OracleParameter("v_cursor", OracleDbType.RefCursor, null, System.Data.ParameterDirection.Output));
            commands.Add(new OracleParameter("v_AreaName", OracleDbType.Varchar2, AreaName, System.Data.ParameterDirection.Input));
            commands.Add(new OracleParameter("v_ControllerName", OracleDbType.Varchar2, ControllerName, System.Data.ParameterDirection.Input));
            commands.Add(new OracleParameter("v_ActionName", OracleDbType.Varchar2, ActionName, System.Data.ParameterDirection.Input));


            DataSet ds = _dbAccess.ExecuteDataSet_ADM("USP_BOB_ADM_MENUMASTER_INSERT", commands);
            return ds;
        }
        public static DataSet SelectMenuMaster(string UserCode, DBAccess _dbAccess)
        {
            List<OracleParameter> commands = new List<OracleParameter>();

            commands.Add(new OracleParameter("v_Code", OracleDbType.Varchar2, UserCode, System.Data.ParameterDirection.Input));
            commands.Add(new OracleParameter("v_cursor", OracleDbType.RefCursor, null, System.Data.ParameterDirection.Output));

            DataSet ds = _dbAccess.ExecuteDataSet_ADM("USP_BOB_ADM_MENUMASTER_SELECT", commands);
            return ds;
        }

        public static DataSet UpdateMenuMaster(string Code, string MenuName, string MenuDesc, string ParentId,
            string MenuUrl, string ControllerName, string ActionName, string MenuIcon, string MenuSrno,
            bool Locked, int LastModifiedBy, string AreaName, DBAccess _dbAccess)
        {
            List<OracleParameter> commands = new List<OracleParameter>();

            commands.Add(new OracleParameter("v_Code", OracleDbType.Int16, Convert.ToInt32(Code), System.Data.ParameterDirection.Input));
            commands.Add(new OracleParameter("v_MenuName", OracleDbType.Varchar2, MenuName, System.Data.ParameterDirection.Input));
            commands.Add(new OracleParameter("v_MenuDesc", OracleDbType.Varchar2, MenuDesc, System.Data.ParameterDirection.Input));
            commands.Add(new OracleParameter("iv_ParentId", OracleDbType.Int16, ParentId, System.Data.ParameterDirection.Input));
            commands.Add(new OracleParameter("v_MenuUrl", OracleDbType.Varchar2, MenuUrl, System.Data.ParameterDirection.Input));
            commands.Add(new OracleParameter("v_AreaName", OracleDbType.Varchar2, AreaName, System.Data.ParameterDirection.Input));
            commands.Add(new OracleParameter("v_ControllerName", OracleDbType.Varchar2, ControllerName, System.Data.ParameterDirection.Input));
            commands.Add(new OracleParameter("v_ActionName", OracleDbType.Varchar2, ActionName, System.Data.ParameterDirection.Input));
            commands.Add(new OracleParameter("v_MenuIcon", OracleDbType.Varchar2, MenuIcon, System.Data.ParameterDirection.Input));
            commands.Add(new OracleParameter("v_MenuSrNo", OracleDbType.Int16, MenuSrno, System.Data.ParameterDirection.Input));
            commands.Add(new OracleParameter("v_Locked", OracleDbType.Int16, Locked ? 1 : 0, System.Data.ParameterDirection.Input));
            commands.Add(new OracleParameter("v_LastModifiedBy", OracleDbType.Int16, Convert.ToInt32(LastModifiedBy), System.Data.ParameterDirection.Input));
            commands.Add(new OracleParameter("v_cursor", OracleDbType.RefCursor, null, System.Data.ParameterDirection.Output));

            DataSet ds = _dbAccess.ExecuteDataSet_ADM("USP_BOB_ADM_MENUMASTER_UPDATE", commands);
            return ds;
        }

        public static DataSet MenuDelete(int Code,int deletedBy, DBAccess _dbAccess)
        {
            List<OracleParameter> commands = new List<OracleParameter>();

            commands.Add(new OracleParameter("v_Code", OracleDbType.Int16, Convert.ToInt32(Code), System.Data.ParameterDirection.Input));
            commands.Add(new OracleParameter("V_DELETED_BY", OracleDbType.Int32, deletedBy, System.Data.ParameterDirection.Input));
            commands.Add(new OracleParameter("v_cursor", OracleDbType.RefCursor, null, System.Data.ParameterDirection.Output));
            DataSet ds = _dbAccess.ExecuteDataSet_ADM("USP_BOB_ADM_MENUMASTER_DELETE", commands);
            return ds;
        }


    }
}
