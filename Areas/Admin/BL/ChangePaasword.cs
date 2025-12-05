using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Oracle.ManagedDataAccess.Client;
using System.Data;
using MasterApplication.Services;
using MasterApplication.DAL;

namespace MasterApplication.Areas.Admin.BL
{
    public class ChangePaasword
    {
        //readonly DependancyInjection DI;

        //public ChangePaasword(DependancyInjection _DI)
        //{
        //    DI = _DI;
        //}

        public static DataSet InsertPass(string username, string password, string re_password, DBAccess dBAccess)
        {
            List<OracleParameter> command = new List<OracleParameter>();
            command.Add(new OracleParameter("p_username",OracleDbType.Varchar2,username, System.Data.ParameterDirection.Input));
            command.Add(new OracleParameter("p_password", OracleDbType.Varchar2, password, System.Data.ParameterDirection.Input));
            command.Add(new OracleParameter("p_cursor", OracleDbType.RefCursor, null, System.Data.ParameterDirection.Output));
            DataSet ds = dBAccess.ExecuteDataSet_ADM("sp_changepassword", command);
            return ds;


        }

    }
}
