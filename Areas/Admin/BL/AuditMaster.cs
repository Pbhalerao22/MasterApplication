using MasterApplication.DAL;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;

namespace MasterApplication.Areas.Admin.BL
{
    public class AuditMaster
    {

        public static DataSet GetTypeList( DBAccess _dbAccess)
        {
            try
            {
                List<OracleParameter> commands = new List<OracleParameter>();

                commands.Add(new OracleParameter("O_CURSOR", OracleDbType.RefCursor, 1, System.Data.ParameterDirection.Output));

                DataSet ds = _dbAccess.ExecuteDataSet_ADM("USP_ADM_DD_GETTYPE", commands);
                return ds;
            }
            catch (Exception)
            {

                throw;
            }
           
        }

        public static DataSet GetListOfUsers(DBAccess _dbAccess)
        {
            try
            {
                Dictionary<string, string> ListOfUser = new Dictionary<string, string>();
                List<OracleParameter> commands = new List<OracleParameter>();

                commands.Add(new OracleParameter("v_cursor", OracleDbType.RefCursor, null, System.Data.ParameterDirection.Output));
                DataSet ds = _dbAccess.ExecuteDataSet_ADM("USP_BOB_ADM_GETALLUSERMASTERFORMAPPING", commands);
                
                return ds;
            }
            catch (Exception)
            {

                throw;
            }
            
        }
        public static DataSet GetAuditDetails(string Type, int usercode, DateTime? from, DateTime? To,DBAccess _dbAccess)
        {

            try
            {
                List<OracleParameter> commands = new List<OracleParameter>();

                commands.Add(new OracleParameter("P_USERCODE", OracleDbType.Int32, usercode, System.Data.ParameterDirection.Input));
                commands.Add(new OracleParameter("P_TYPE", OracleDbType.Varchar2, Type, System.Data.ParameterDirection.Input));
                commands.Add(new OracleParameter("P_FROMDATE", OracleDbType.Date, from, System.Data.ParameterDirection.Input));
                commands.Add(new OracleParameter("P_TODATE", OracleDbType.Date, To, System.Data.ParameterDirection.Input));
                commands.Add(new OracleParameter("O_CURSOR", OracleDbType.RefCursor, null, System.Data.ParameterDirection.Output));
                commands.Add(new OracleParameter("O_CURSOR_1", OracleDbType.RefCursor, null, System.Data.ParameterDirection.Output));

                DataSet ds = _dbAccess.ExecuteDataSet_ADM("USP_ADM_GET_AUDIT_DETAILS", commands);
                return ds;
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
