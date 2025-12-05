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
    public class SecurityQuestions
    {


        public static List<SecurityQuestionsModel> GetSecurityQuestions(DBAccess _dbAccess)
        {
            List<OracleParameter> commands = new List<OracleParameter>();
            commands.Add(new OracleParameter("v_cursor", OracleDbType.RefCursor, null, System.Data.ParameterDirection.Output));
            DataSet dataSet = _dbAccess.ExecuteDataSet("BYT_PGETSECURITYQUESTIONS", commands);
            List<SecurityQuestionsModel> securityQuestions = new List<SecurityQuestionsModel>();
            for (int i = 0; i < dataSet.Tables[0].Rows.Count; i++)
            {
                securityQuestions.Add(new SecurityQuestionsModel
                {
                    Question = dataSet.Tables[0].Rows[i]["Question"].ToString(),
                    CODE = (dataSet.Tables[0].Rows[i]["Code"].ToString())
                });
            }
            return securityQuestions;
        }

        public static DataSet InsertSecurityQuestions(string Question, DBAccess _dbAccess)
        {

            List<OracleParameter> commands = new List<OracleParameter>();

            commands.Add(new OracleParameter("iv_Code", OracleDbType.Int16, 1, System.Data.ParameterDirection.Input));
            commands.Add(new OracleParameter("v_Question", OracleDbType.Varchar2, Question, System.Data.ParameterDirection.Input));
            commands.Add(new OracleParameter("v_cursor", OracleDbType.RefCursor, null, System.Data.ParameterDirection.Output));

            DataSet ds = _dbAccess.ExecuteDataSet_ADM("USP_BOB_ADM_SECURITYQUESTIONS_INSERT", commands);
            return ds;
        }

        public static DataSet SelectSecurityQuestions(string Code, DBAccess _dbAccess)
        {
            List<OracleParameter> commands = new List<OracleParameter>();

            commands.Add(new OracleParameter("v_Code", OracleDbType.Varchar2, Code, System.Data.ParameterDirection.Input));
            commands.Add(new OracleParameter("v_cursor", OracleDbType.RefCursor, null, System.Data.ParameterDirection.Output));

            DataSet ds = _dbAccess.ExecuteDataSet_ADM("USP_BOB_SECURITYQUESTIONS_SELECT", commands);
            return ds;
        }

        public static DataSet UpdateSecurityQuestions(string Code, string Question, bool Locked, DBAccess _dbAccess)
        {
            List<OracleParameter> commands = new List<OracleParameter>();

            commands.Add(new OracleParameter("v_Code", OracleDbType.Int16, Convert.ToInt32(Code), System.Data.ParameterDirection.Input));
            commands.Add(new OracleParameter("v_Question", OracleDbType.Varchar2, Question, System.Data.ParameterDirection.Input));
            commands.Add(new OracleParameter("v_Locked", OracleDbType.Int16, Locked ? 1 : 0, System.Data.ParameterDirection.Input));
            commands.Add(new OracleParameter("v_cursor", OracleDbType.RefCursor, null, System.Data.ParameterDirection.Output));

            DataSet ds = _dbAccess.ExecuteDataSet_ADM("USP_BOB_ADM_SECURITYQUESTIONS_UPDATE", commands);
            return ds;
        }


        public static DataSet SecurityQuestionsDelete(int Code, DBAccess _dbAccess)
        {
            List<OracleParameter> commands = new List<OracleParameter>();
            commands.Add(new OracleParameter("v_Code", OracleDbType.Int16, Convert.ToInt32(Code), System.Data.ParameterDirection.Input));
            commands.Add(new OracleParameter("v_cursor", OracleDbType.RefCursor, null, System.Data.ParameterDirection.Output));
            DataSet ds = _dbAccess.ExecuteDataSet_ADM("USP_BOB_ADM_SECURITYQUESTIONS_DELETE", commands);
            return ds;
        }
    }
}
