using MasterApplication.Services;
using Microsoft.Extensions.Options;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace MasterApplication.DAL
{
    public class DBAccess
    {
        readonly MyAppSettings myAppSettings;
        public DBAccess()
        {

        }
        public DBAccess(IOptions<MyAppSettings> options)
        {
            myAppSettings = options.Value;
        }
        public DataSet ExecuteQueryOrcale(string Query)
        {
            DataSet dsResult = new DataSet();
            try
            {

                string _stcon = myAppSettings.OracleConnection;
                string stcon = MSEncrypto.Encryption.Decrypt(_stcon);

                OracleConnection conn = new OracleConnection(stcon);

                OracleCommand cmd = conn.CreateCommand();
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = Query;

                conn.Open();

                cmd.ExecuteNonQuery();
                conn.Close();

                return dsResult;
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }
        public DataSet ExecuteDataSet_ADM(string procName, List<OracleParameter> lstParams)
        {
            bool UseSqlConnection = Convert.ToBoolean(myAppSettings.UseSqlConnection);
            if (UseSqlConnection)
            {
                List<SqlParameter> sqlParameters = new List<SqlParameter>();
                foreach (var OracleParam in lstParams)
                {
                    if (OracleParam.OracleDbType != OracleDbType.RefCursor)
                    {
                        if (OracleParam.Value == null)
                        {
                            sqlParameters.Add(new SqlParameter(OracleParam.ParameterName, DBNull.Value));
                        }
                        else
                        {
                            sqlParameters.Add(new SqlParameter(OracleParam.ParameterName, OracleParam.Value));
                        }
                    }
                }
                return ExecuteDataSet_Admin_SQL(procName, sqlParameters);
            }
            else
            {
                return ExecuteDataSet_Admin_Oracle(procName, lstParams);
            }
        }
        public DataSet ExecuteDataSet_Admin_SQL(string procName, List<SqlParameter> lstParams)
        {
            DataSet dsResult = new DataSet();
            try
            {
                string _stcon = myAppSettings.SQLConnection_ADM;
                string stcon = MSEncrypto.Encryption.Decrypt(_stcon);
                using (SqlConnection sqlCon = new SqlConnection(stcon))
                {
                    sqlCon.Open();
                    SqlCommand sql_cmnd = new SqlCommand(procName, sqlCon);
                    sql_cmnd.CommandType = CommandType.StoredProcedure;
                    foreach (var SQlParam in lstParams)
                    {
                        sql_cmnd.Parameters.AddWithValue(SQlParam.ParameterName, SQlParam.Value);
                    }

                    SqlDataAdapter adapter = new SqlDataAdapter(sql_cmnd);
                    adapter.Fill(dsResult);

                    sqlCon.Close();
                    return dsResult;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet ExecuteDataSet(string procName, List<OracleParameter> lstParams)
        {
            DataSet dsResult = new DataSet();
            try
            {
                string _stcon = myAppSettings.OracleConnection;
                string stcon = MSEncrypto.Encryption.Decrypt(_stcon);

                OracleConnection conn = new OracleConnection(stcon);

                OracleCommand cmd = conn.CreateCommand();
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.CommandText = procName;

                conn.Open();

                if (lstParams != null)
                {
                    foreach (OracleParameter oracleParameter in lstParams)
                    {
                        cmd.Parameters.Add(oracleParameter);
                    }
                }
                OracleDataAdapter adapter = new OracleDataAdapter(cmd);

                adapter.Fill(dsResult);
                conn.Close();

                return dsResult;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public DataSet ExecuteDataSet_Admin_Oracle(string procName, List<OracleParameter> lstParams)
        {
            DataSet dsResult = new DataSet();
            try
            {
                string _stcon = myAppSettings.OracleConnection_ADM;
                string stcon = MSEncrypto.Encryption.Decrypt(_stcon);

                OracleConnection conn = new OracleConnection(stcon);

                OracleCommand cmd = conn.CreateCommand();
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.CommandText = procName;

                conn.Open();

                if (lstParams != null)
                {
                    foreach (OracleParameter oracleParameter in lstParams)
                    {
                        cmd.Parameters.Add(oracleParameter);
                    }
                }
                OracleDataAdapter adapter = new OracleDataAdapter(cmd);

                adapter.Fill(dsResult);
                conn.Close();

                return dsResult;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public void BulkDataTableUpload(DataTable dataTable, string TableName)
        {
            try
            {
                string _stcon = myAppSettings.OracleConnection;
                string stcon = MSEncrypto.Encryption.Decrypt(_stcon);

                OracleConnection conn = new OracleConnection(stcon);
                conn.Open();
                using (OracleBulkCopy obc = new OracleBulkCopy(conn))
                {
                    obc.BatchSize = Convert.ToInt16(myAppSettings.BatchSize);
                    obc.DestinationTableName = TableName;
                    obc.WriteToServer(dataTable);
                    obc.Close();
                }
                conn.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public void WriteErrorLog(string Message)
        {
            StreamWriter sw = null;
            try
            {
                string strDirectory = AppDomain.CurrentDomain.BaseDirectory + @"\Logs";
                if (!Directory.Exists(strDirectory))
                {
                    Directory.CreateDirectory(strDirectory);
                }
                sw = new StreamWriter(strDirectory + "\\LogFile" + DateTime.Now.ToString("dd_MM_yyyy") + ".txt", true);
                sw.WriteLine(DateTime.Now.ToString() + " ==>  " + Message);
                sw.Flush();
                sw.Close();
            }
            catch
            {
            }
        }
        public string ProjectSetting_GetValue(string KeyName, string ProjectName, DependancyInjection DI)
        {
            List<OracleParameter> commands = new List<OracleParameter>();
            commands.Add(new OracleParameter("P_PROJECTNAME", OracleDbType.Varchar2, ProjectName, ParameterDirection.Input));
            commands.Add(new OracleParameter("P_KEYNAME", OracleDbType.Varchar2, KeyName, ParameterDirection.Input));
            commands.Add(new OracleParameter("P_OUTPUTRESULTSET", OracleDbType.RefCursor, null, ParameterDirection.Output));

            DataSet ds = DI.dBAccess.ExecuteDataSet("USP_GET_PROJECTSETTING_VALUE", commands);
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                return ds.Tables[0].Rows[0]["KEYVALUE"].ToString();
            }
            return "";
        }
        public void UPI_FundTransfer_RequestLog(string strText, string RRN, string ReqType, string APIType)
        {
            StreamWriter sw = null;
            try
            {
                string strDirectory = AppDomain.CurrentDomain.BaseDirectory + @"\RequestLog\" + APIType + "\\" + DateTime.Now.ToString("yyyyMMdd");
                if (!Directory.Exists(strDirectory))
                {
                    Directory.CreateDirectory(strDirectory);
                }
                sw = new StreamWriter(strDirectory + "\\" + RRN + "_" + ReqType + ".txt", true);
                sw.WriteLine(ReqType + " ==>  " + strText);
                sw.Flush();
                sw.Close();
            }
            catch
            {

            }
        }
        public string DecryptCode(string strEncryptedString)
        {
            string strEnc = MSEncrypto.Encryption.Decrypt(strEncryptedString);
            return strEnc;
        }
        public bool Upload_File_Details(string Filename, string ProjectName, string status, string FileType, DependancyInjection DI)
        {
            try
            {
                DataSet ds = new DataSet();
                List<OracleParameter> lstParams = new List<OracleParameter>();

                lstParams.Add(new OracleParameter("P_FILENAME", OracleDbType.Varchar2, Filename, ParameterDirection.Input));
                lstParams.Add(new OracleParameter("P_PROJECTNAME", OracleDbType.Varchar2, ProjectName, ParameterDirection.Input));
                lstParams.Add(new OracleParameter("P_STATUS", OracleDbType.Varchar2, status, ParameterDirection.Input));
                lstParams.Add(new OracleParameter("P_FILE_TYPE", OracleDbType.Varchar2, FileType, ParameterDirection.Input));

                ds = DI.dBAccess.ExecuteDataSet("USP_TBL_FILE_UPLOAD_DETAILS_INSERT", lstParams);
                return true;
            }
            catch (Exception ex)
            {
                return false;
                //throw ex;
            }
        }
        public bool CheckFileExists(string strProjName, string strFileName, DependancyInjection DI)
        {
            List<OracleParameter> lstParams = new List<OracleParameter>();

            lstParams.Add(new OracleParameter("P_PROJECTNAME", OracleDbType.Varchar2, strProjName, ParameterDirection.Input));
            lstParams.Add(new OracleParameter("P_FILENAME", OracleDbType.Varchar2, strFileName, ParameterDirection.Input));
            lstParams.Add(new OracleParameter("P_REFCURSOR", OracleDbType.RefCursor, null, ParameterDirection.Output));

            DataSet ds = DI.dBAccess.ExecuteDataSet("USP_TBL_FILE_UPLOAD_DETAILS_SELECT", lstParams);
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public DataSet ExecuteDataSet_LMS(string procName, List<OracleParameter> lstParams)
        {
            DataSet dsResult = new DataSet();
            try
            {
                string _stcon = myAppSettings.LMS_OracleConnection;
                string stcon = MSEncrypto.Encryption.Decrypt(_stcon);

                OracleConnection conn = new OracleConnection(stcon);

                OracleCommand cmd = conn.CreateCommand();
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.CommandText = procName;

                conn.Open();

                if (lstParams != null)
                {
                    foreach (OracleParameter oracleParameter in lstParams)
                    {
                        cmd.Parameters.Add(oracleParameter);
                    }
                }
                OracleDataAdapter adapter = new OracleDataAdapter(cmd);

                adapter.Fill(dsResult);
                conn.Close();

                return dsResult;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public  string ProjectSetting_Fetch(string ProjectName, string KeyName, DependancyInjection DI)
        {
            DataSet dsProjSetting = new DataSet();
            List<OracleParameter> lstParams = new List<OracleParameter>();

            lstParams.Add(new OracleParameter("P_PROJECTNAME", OracleDbType.Varchar2, ProjectName, ParameterDirection.Input));
            lstParams.Add(new OracleParameter("P_KEYNAME", OracleDbType.Varchar2, KeyName, ParameterDirection.Input));
            lstParams.Add(new OracleParameter("P_OUTPUTRESULTSET", OracleDbType.RefCursor, null, ParameterDirection.Output));

            dsProjSetting = DI.dBAccess.ExecuteDataSet("USP_GET_PROJECTSETTING_VALUE", lstParams); //ExecuteOracleDataSet("USP_GET_PROJECTSETTING_VALUE", lstParams, appSettings);
            if (dsProjSetting != null && dsProjSetting.Tables.Count > 0 && dsProjSetting.Tables[0].Rows.Count > 0)
            {
                return dsProjSetting.Tables[0].Rows[0]["KEYVALUE"].ToString();
            }
            else
            {
                return "";
            }
        }
    }
}
