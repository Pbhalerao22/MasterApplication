using MasterApplication.DAL;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Oracle.ManagedDataAccess.Client;
using Org.BouncyCastle.Asn1.Ocsp;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace MasterApplication.Services
{
    public class CommonClass
    {/*
        readonly DependancyInjection DI;
        public CommonClass(DependancyInjection _dependancyInjection)
        {
            DI = _dependancyInjection;
        }
        */
        private static HttpRequest request;

        public DataSet GetMasterForGrid_ADM(string strSearchValue, string strSearchColumn,
           string strOrderByColumn, string strOrderByType, int iPageNo, int iPageNoOfRows, string ViewName, DependancyInjection DI)
        {
            List<OracleParameter> commands = new List<OracleParameter>();

            commands.Add(new OracleParameter("p_viewname", OracleDbType.Varchar2, ViewName, System.Data.ParameterDirection.Input));
            commands.Add(new OracleParameter("p_SearchColumn", OracleDbType.Varchar2, strSearchColumn, System.Data.ParameterDirection.Input));
            commands.Add(new OracleParameter("p_SearchValue", OracleDbType.Varchar2, strSearchValue, System.Data.ParameterDirection.Input));
            commands.Add(new OracleParameter("p_OrderByCol", OracleDbType.Varchar2, strOrderByColumn, System.Data.ParameterDirection.Input));
            commands.Add(new OracleParameter("p_OrderByType", OracleDbType.Varchar2, strOrderByType, System.Data.ParameterDirection.Input));
            commands.Add(new OracleParameter("p_PageNo", OracleDbType.Varchar2, iPageNo, System.Data.ParameterDirection.Input));
            commands.Add(new OracleParameter("p_NoOfRows", OracleDbType.Varchar2, iPageNoOfRows, System.Data.ParameterDirection.Input));
            commands.Add(new OracleParameter("v_CursorToalRecords", OracleDbType.RefCursor, null, System.Data.ParameterDirection.Output));
            commands.Add(new OracleParameter("v_CursorFetchRecords", OracleDbType.RefCursor, null, System.Data.ParameterDirection.Output));

            DataSet ds = DI.dBAccess.ExecuteDataSet_ADM("USP_BOB_COMMONGRIDDATA", commands);
            return ds;
        }

        public DataSet GetMasterForGrid(string strSearchValue, string strSearchColumn,
           string strOrderByColumn, string strOrderByType, int iPageNo, int iPageNoOfRows, string ViewName, DependancyInjection DI)
        {
            List<OracleParameter> commands = new List<OracleParameter>();

            commands.Add(new OracleParameter("p_viewname", OracleDbType.Varchar2, ViewName, System.Data.ParameterDirection.Input));
            commands.Add(new OracleParameter("p_SearchColumn", OracleDbType.Varchar2, strSearchColumn, System.Data.ParameterDirection.Input));
            commands.Add(new OracleParameter("p_SearchValue", OracleDbType.Varchar2, strSearchValue, System.Data.ParameterDirection.Input));
            commands.Add(new OracleParameter("p_OrderByCol", OracleDbType.Varchar2, strOrderByColumn, System.Data.ParameterDirection.Input));
            commands.Add(new OracleParameter("p_OrderByType", OracleDbType.Varchar2, strOrderByType, System.Data.ParameterDirection.Input));
            commands.Add(new OracleParameter("p_PageNo", OracleDbType.Varchar2, iPageNo, System.Data.ParameterDirection.Input));
            commands.Add(new OracleParameter("p_NoOfRows", OracleDbType.Varchar2, iPageNoOfRows, System.Data.ParameterDirection.Input));
            commands.Add(new OracleParameter("v_CursorToalRecords", OracleDbType.RefCursor, null, System.Data.ParameterDirection.Output));
            commands.Add(new OracleParameter("v_CursorFetchRecords", OracleDbType.RefCursor, null, System.Data.ParameterDirection.Output));

            DataSet ds = DI.dBAccess.ExecuteDataSet("USP_BOB_COMMONGRIDDATA", commands);
            return ds;
        }

        public DataSet GetMasterForGridRoleMap(string strSearchValue, string strSearchColumn,
               string strOrderByColumn, string strOrderByType, int iPageNo, int iPageNoOfRows, string ViewName, string strRoleCode, DependancyInjection DI)
        {

            List<OracleParameter> commands = new List<OracleParameter>();

            commands.Add(new OracleParameter("p_viewname", OracleDbType.Varchar2, ViewName, System.Data.ParameterDirection.Input));
            commands.Add(new OracleParameter("p_SearchColumn", OracleDbType.Varchar2, strSearchColumn, System.Data.ParameterDirection.Input));
            commands.Add(new OracleParameter("p_SearchValue", OracleDbType.Varchar2, strSearchValue, System.Data.ParameterDirection.Input));
            commands.Add(new OracleParameter("p_OrderByCol", OracleDbType.Varchar2, strOrderByColumn, System.Data.ParameterDirection.Input));
            commands.Add(new OracleParameter("p_OrderByType", OracleDbType.Varchar2, strOrderByType, System.Data.ParameterDirection.Input));
            commands.Add(new OracleParameter("p_PageNo", OracleDbType.Varchar2, iPageNo, System.Data.ParameterDirection.Input));
            commands.Add(new OracleParameter("p_NoOfRows", OracleDbType.Varchar2, iPageNoOfRows, System.Data.ParameterDirection.Input));
            commands.Add(new OracleParameter("p_RoleCode", OracleDbType.Varchar2, strRoleCode, System.Data.ParameterDirection.Input));
            commands.Add(new OracleParameter("v_CursorToalRecords", OracleDbType.RefCursor, null, System.Data.ParameterDirection.Output));
            commands.Add(new OracleParameter("v_CursorFetchRecords", OracleDbType.RefCursor, null, System.Data.ParameterDirection.Output));

            DataSet ds = DI.dBAccess.ExecuteDataSet("USP_BOB_COMMONGRID_ROLEMENUMAP", commands);
            return ds;
        }
        public DataSet GetMasterForGridRoleMap_ADM(string strSearchValue, string strSearchColumn,
               string strOrderByColumn, string strOrderByType, int iPageNo, int iPageNoOfRows, string ViewName, string strRoleCode, DependancyInjection DI)
        {

            List<OracleParameter> commands = new List<OracleParameter>();

            commands.Add(new OracleParameter("p_viewname", OracleDbType.Varchar2, ViewName, System.Data.ParameterDirection.Input));
            commands.Add(new OracleParameter("p_SearchColumn", OracleDbType.Varchar2, strSearchColumn, System.Data.ParameterDirection.Input));
            commands.Add(new OracleParameter("p_SearchValue", OracleDbType.Varchar2, strSearchValue, System.Data.ParameterDirection.Input));
            commands.Add(new OracleParameter("p_OrderByCol", OracleDbType.Varchar2, strOrderByColumn, System.Data.ParameterDirection.Input));
            commands.Add(new OracleParameter("p_OrderByType", OracleDbType.Varchar2, strOrderByType, System.Data.ParameterDirection.Input));
            commands.Add(new OracleParameter("p_PageNo", OracleDbType.Varchar2, iPageNo, System.Data.ParameterDirection.Input));
            commands.Add(new OracleParameter("p_NoOfRows", OracleDbType.Varchar2, iPageNoOfRows, System.Data.ParameterDirection.Input));
            commands.Add(new OracleParameter("p_RoleCode", OracleDbType.Varchar2, strRoleCode, System.Data.ParameterDirection.Input));
            commands.Add(new OracleParameter("v_CursorToalRecords", OracleDbType.RefCursor, null, System.Data.ParameterDirection.Output));
            commands.Add(new OracleParameter("v_CursorFetchRecords", OracleDbType.RefCursor, null, System.Data.ParameterDirection.Output));

            DataSet ds = DI.dBAccess.ExecuteDataSet_ADM("USP_BOB_COMMONGRID_ROLEMENUMAP", commands);
            return ds;
        }
        public DataSet GetGetColumnNames(string strTableName, DBAccess dBAccess)
        {
            List<OracleParameter> lstParams = new List<OracleParameter>();
            lstParams.Add(new OracleParameter("p_TableName", OracleDbType.Varchar2, strTableName, ParameterDirection.Input));
            lstParams.Add(new OracleParameter("P_resultset", OracleDbType.RefCursor, null, ParameterDirection.Output));
            DataSet dsCols = dBAccess.ExecuteDataSet("USP_BOB_GETCOLUMNNAME", lstParams);
            return dsCols;
        }
        public DataSet TruncateRawTable(string strTableName, DBAccess dBAccess)
        {
            List<OracleParameter> lstParams = new List<OracleParameter>();
            lstParams.Add(new OracleParameter("i_TableName", OracleDbType.Varchar2, strTableName, ParameterDirection.Input));
            lstParams.Add(new OracleParameter("i_OutputResult", OracleDbType.RefCursor, null, ParameterDirection.Output));
            //DataSet dsTrunc = dBAccess.ExecuteDataSet("USP_BOB_TRUNCATETABLE_RAW", lstParams);
            DataSet dsTrunc = dBAccess.ExecuteDataSet("USP_BOB_TRUNCATETABLES", lstParams);
            return dsTrunc;
        }
        public DataSet GetFilePath(string strProjectName, string strFileType, DBAccess dBAccess)
        {
            DataSet ds = new DataSet();
            List<OracleParameter> lstParams = new List<OracleParameter>();

            lstParams.Add(new OracleParameter("P_ProjectName", OracleDbType.Varchar2, strProjectName, ParameterDirection.Input));
            lstParams.Add(new OracleParameter("P_FileType", OracleDbType.Varchar2, strFileType, ParameterDirection.Input));
            lstParams.Add(new OracleParameter("O_VALUE", OracleDbType.RefCursor, null, ParameterDirection.Output));

            DataSet DataSet = dBAccess.ExecuteDataSet("USP_BOB_GET_FILE_PATH", lstParams);
            return DataSet;
        }
 
        public void ProcessLogInsertUpdate(string ProcessType, string FileName, string Status, string StatusDesc, string SessionID, string DMLType, string ServiceType, DBAccess dBAccess)
        {
            DataSet ds = new DataSet();
            List<OracleParameter> lstParams = new List<OracleParameter>();

            lstParams.Add(new OracleParameter("P_ProcessType", OracleDbType.Varchar2, ProcessType, ParameterDirection.Input));
            lstParams.Add(new OracleParameter("P_FileName", OracleDbType.Varchar2, FileName, ParameterDirection.Input));
            lstParams.Add(new OracleParameter("P_Status", OracleDbType.Varchar2, Status, ParameterDirection.Input));
            lstParams.Add(new OracleParameter("P_StatusDesc", OracleDbType.Varchar2, StatusDesc, ParameterDirection.Input));
            lstParams.Add(new OracleParameter("P_SessionID", OracleDbType.Varchar2, SessionID, ParameterDirection.Input));
            // lstParams.Add(new OracleParameter("P_InsertUpdate", OracleDbType.Varchar2, DMLType, ParameterDirection.Input));
            lstParams.Add(new OracleParameter("P_serviceType", OracleDbType.Varchar2, ServiceType, ParameterDirection.Input));

            DataSet DataSet = dBAccess.ExecuteDataSet("USP_BOB_PROCESSLOGINSERTUPDATE", lstParams);
        }
     
        public DataSet GetMasterForGrid_UPP(string Query,
          //string strSearchValue, string strSearchColumn,
          string strOrderByColumn, string strOrderByType, int iPageNo, int iPageNoOfRows, string ViewName, DependancyInjection DI)
        {
            List<OracleParameter> commands = new List<OracleParameter>();

            commands.Add(new OracleParameter("p_viewname", OracleDbType.Varchar2, ViewName, System.Data.ParameterDirection.Input));
            //commands.Add(new OracleParameter("p_SearchColumn", OracleDbType.Varchar2, strSearchColumn, System.Data.ParameterDirection.Input));
            //commands.Add(new OracleParameter("p_SearchValue", OracleDbType.Varchar2, strSearchValue, System.Data.ParameterDirection.Input));
            commands.Add(new OracleParameter("p_OrderByCol", OracleDbType.Varchar2, strOrderByColumn, System.Data.ParameterDirection.Input));
            commands.Add(new OracleParameter("p_OrderByType", OracleDbType.Varchar2, strOrderByType, System.Data.ParameterDirection.Input));
            commands.Add(new OracleParameter("p_PageNo", OracleDbType.Varchar2, iPageNo, System.Data.ParameterDirection.Input));
            commands.Add(new OracleParameter("p_NoOfRows", OracleDbType.Varchar2, iPageNoOfRows, System.Data.ParameterDirection.Input));
            commands.Add(new OracleParameter("p_Query", OracleDbType.Varchar2, Query, System.Data.ParameterDirection.Input));
            commands.Add(new OracleParameter("v_CursorToalRecords", OracleDbType.RefCursor, null, System.Data.ParameterDirection.Output));
            commands.Add(new OracleParameter("v_CursorFetchRecords", OracleDbType.RefCursor, null, System.Data.ParameterDirection.Output));

            DataSet ds = DI.dBAccess.ExecuteDataSet("USP_UPP_SEARCHDATA", commands);
            return ds;
        }


        //Utkarsh 18062025 SendMail,Get_Details,EmailBody
        public static void SendMail(DependancyInjection DI, DataSet dsResult, string Projectname, string strMailType, string strNextTime = "")
        {
            string mentorguid = Guid.NewGuid().ToString();
            try
            {
                string EmailTemplatePath = DI.dBAccess.ProjectSetting_GetValue("EmailTemplatePath", Projectname, DI);
                string SmtpClient = DI.dBAccess.ProjectSetting_GetValue("SmtpClient", Projectname, DI);
                string FromMailAddress = DI.dBAccess.ProjectSetting_GetValue("FromMailAddress", Projectname, DI);
                string ToMail = DI.dBAccess.ProjectSetting_GetValue("ToMail", Projectname, DI);
                string MailSubject = DI.dBAccess.ProjectSetting_GetValue("MailSubject", Projectname, DI);
                string Port = DI.dBAccess.ProjectSetting_GetValue("Port", Projectname, DI);
                string UseDefaultCredentials = DI.dBAccess.ProjectSetting_GetValue("UseDefaultCredentials", Projectname, DI);
                string SendMail_Bank_Dev = DI.dBAccess.ProjectSetting_GetValue("SendMail_Bank_Dev", Projectname, DI);
                string NetworkCredentialEmail = DI.dBAccess.ProjectSetting_GetValue("NetworkCredentialEmail", Projectname, DI);
                string NetworkCredentialPswd = DI.dBAccess.ProjectSetting_GetValue("NetworkCredentialPswd", Projectname, DI);
                string Timeout = DI.dBAccess.ProjectSetting_GetValue("Timeout", Projectname, DI);
                string EnableSsl = DI.dBAccess.ProjectSetting_GetValue("EnableSsl", Projectname, DI);



                if (SendMail_Bank_Dev == "1")
                {
                    SendEmail_Bank(dsResult.Tables[0], Projectname, "", ToMail, MailSubject, DI, EmailTemplatePath, strMailType, strNextTime);
                }
                else if (SendMail_Bank_Dev == "2")
                {
                    string strTable = "";
                    string htmltemplate = System.IO.File.ReadAllText(EmailTemplatePath);

                    DateTime date = DateTime.Now;
                    string FILENAME = null;
                    int File_record_count = 0;
                    int RTMrecord_count = 0;
                    string file_status = null;



                    if (strMailType == "ETL")
                    {
                        //htmltemplate = htmltemplate.Replace("$var1$", "Card Renewal");
                        //htmltemplate = htmltemplate.Replace("$var2$", DateTime.Now.ToString("dd-MM-yyyy") + " " + strNextTime);
                    }
                    else if (strMailType == "DAILY_STATUS")
                    {
                        if (dsResult != null && dsResult.Tables[0].Rows.Count > 0)
                        {
                            strTable += EmailBody(dsResult.Tables[0], htmltemplate,DI);
                            htmltemplate = strTable;
                        }
                    }
                    else if (strMailType == "FILE_SUMMARY")
                    {
                        if (dsResult != null && dsResult.Tables[0].Rows.Count > 0)
                        {
                            strTable += EmailBody(dsResult.Tables[0], htmltemplate, DI);
                            htmltemplate = strTable;
                        }
                    }
                    MailMessage mail = new MailMessage();
                    SmtpClient SmtpServer = new SmtpClient();

                    try
                    {
                        mail.From = new MailAddress(FromMailAddress);
                        mail.To.Add(ToMail);
                        mail.Subject = MailSubject + " " + DateTime.Now.ToString("dd-MM-yyyy");
                        mail.Body = htmltemplate;
                        mail.IsBodyHtml = true;
                        SmtpServer.Port = Convert.ToInt32(Port);
                        SmtpServer.UseDefaultCredentials = Convert.ToBoolean(UseDefaultCredentials);
                        SmtpServer.Host = SmtpClient;
                        SmtpServer.Credentials = new System.Net.NetworkCredential(NetworkCredentialEmail, NetworkCredentialPswd);
                        SmtpServer.Timeout = Convert.ToInt32(Timeout);
                        SmtpServer.EnableSsl = Convert.ToBoolean(EnableSsl);
                        SmtpServer.Send(mail);
                    }
                    catch (Exception ex)
                    {

                    }
                }
            }
            catch (Exception ex1)
            {
                throw;
            }

        }


        public static void SendEmail_Bank(DataTable dtRecords, string strProjectName, string strProcessSubType, string strEmailID,
         string strMailSubj, DependancyInjection DI, string EmailTemplatePath, string strMailType, string strNextTime = "")
        {
            #region
            DataSet dsRecords = new DataSet();

            string strTable = "";
            string strUserID = "";
            string strPassword = "";
            string strSubID = "";
            string strDecode = "";
            string strMSGType = "";
            string strCType = "";
            string strTempName = "";
            string strURL = "";
            string strMailContent = "";
            string strCCMail = "";
            string strBCCMail = "";

            string[] tmpTo = { };
            string[] CCTo = { };
            string[] BCCTo = { };

            #endregion            
            try
            {
                string RequestFrom = DI.dBAccess.ProjectSetting_GetValue("RequestFrom", strProjectName, DI);
                string FromMailBank = DI.dBAccess.ProjectSetting_GetValue("FromMailBank", strProjectName, DI);
                string APIURL = DI.dBAccess.ProjectSetting_GetValue("SENDEMAIL_API", strProjectName, DI);

                dsRecords = Get_ACL_Details(DI, RequestFrom);

                if (dsRecords != null && dsRecords.Tables.Count > 0 && dsRecords.Tables[0].Rows.Count > 0)
                {
                    strUserID = dsRecords.Tables[0].Rows[0]["USERID"].ToString();
                    strPassword = dsRecords.Tables[0].Rows[0]["PASSWORD"].ToString();
                    strSubID = dsRecords.Tables[0].Rows[0]["SUBID"].ToString();
                    strDecode = dsRecords.Tables[0].Rows[0]["DCODE"].ToString();
                    strMSGType = dsRecords.Tables[0].Rows[0]["MSGTYPE"].ToString();
                    strCType = dsRecords.Tables[0].Rows[0]["CTYPE"].ToString();
                    strTempName = dsRecords.Tables[0].Rows[0]["TEMPNAME"].ToString();
                    strURL = dsRecords.Tables[0].Rows[0]["URL"].ToString();
                }

                strMailContent = File.ReadAllText(EmailTemplatePath);
                string FromMail = FromMailBank;
                string strToEmail = strEmailID;

                 if (strMailType == "FILE_SUMMARY")
                {
                    if (dtRecords != null && dtRecords.Rows.Count > 0)
                    {
                        strTable += EmailBody(dtRecords, strMailContent,DI);
                        strMailContent = strTable;
                    }
                }
                if (strToEmail != "")
                {
                    List<To> lstTo = new List<To>();
                    tmpTo = strToEmail.Split(',');
                    for (int i = 0; i < tmpTo.Length; i++)
                    {
                        lstTo.Add(new To(tmpTo[i]));
                    }
                }
                else
                {
                    tmpTo = new string[] { };
                }

                if (strCCMail != "")
                {
                    List<CC> mstTo = new List<CC>();
                    CCTo = strCCMail.Split(',');
                    for (int i = 0; i < CCTo.Length; i++)
                    {
                        mstTo.Add(new CC(CCTo[i]));
                    }
                }
                else
                {
                    CCTo = new string[] { };
                }

                if (strBCCMail != "")
                {
                    List<BCC> kstTo = new List<BCC>();
                    BCCTo = strBCCMail.Split(',');
                    for (int i = 0; i < BCCTo.Length; i++)
                    {
                        kstTo.Add(new BCC(BCCTo[i]));
                    }
                }
                else
                {
                    BCCTo = new string[] { };
                }

                string[] emptyVar = new string[] { };

                Email_Json email_obj = new Email_Json();

                email_obj.userid = strUserID;
                email_obj.pwd = strPassword;
                email_obj.subuid = strSubID;
                email_obj.dcode = strDecode;
                email_obj.to = tmpTo;
                email_obj.cc = CCTo;
                email_obj.bcc = BCCTo;
                email_obj.msgtype = strMSGType;
                email_obj.from = FromMail;
                email_obj.subjectline = strMailSubj;
                email_obj.ctype = strCType;
                email_obj.msgtxt = strMailContent;
                email_obj.tempname = strTempName;
                email_obj.variables = emptyVar.ToArray();

                string jsonString_email_obj = JsonConvert.SerializeObject(email_obj);

                var requestemail = new
                {
                    email = new
                    {
                        requestType = RequestFrom,
                        Mailbody = JsonConvert.DeserializeObject(jsonString_email_obj),
                        Api_Caller = strProjectName
                    }
                };

                string jsonstring1 = JsonConvert.SerializeObject(requestemail);


                byte[] requestInFormOfBytes = Encoding.ASCII.GetBytes(jsonstring1);

                var httpWebRequest = (HttpWebRequest)WebRequest.Create(APIURL);
                httpWebRequest.ContentType = "application/json";
                httpWebRequest.Method = "POST";
                httpWebRequest.ContentLength = requestInFormOfBytes.Length;

                Stream webrequestStream = httpWebRequest.GetRequestStream();
                webrequestStream.Write(requestInFormOfBytes, 0, requestInFormOfBytes.Length);
                webrequestStream.Close();

                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                Stream responseStream = httpResponse.GetResponseStream();
                StreamReader streader = new StreamReader(responseStream, Encoding.UTF8);
                string strResult = streader.ReadToEnd();


            }
            catch (Exception ex)
            {
                FormsAuthentication.LogException(ex, request, DI.session, "NFSAutoRevesal", "FileUpload", DI.dBAccess);

            }
        }

        //Added For LCR
		public static void SendEmail_Moksha(DataTable dtRecords,  string strProjectName, string strProcessSubType, string strEmailids,
		string strMailSubj, string strMailType,string strMokshaID,string strMokshaPass,string strMailPath,string UserName,string strDate, List<string> FilePath, DependancyInjection DI)
		{
			#region Declaration
			DataSet dsPath = new DataSet();

			string strGUID = "";
			string strEmailID = "";
	
			string strMailContent = "";
			string strTable = "";
			#endregion
			try
			{
				strGUID = Guid.NewGuid().ToString();

			//	DAL.WriteLog(strProcessName + " Email Sending Process Started", appSettings, strServiceType);
				//DAL.ProcessLogInsert(strProcessName + " Email Process", "", "P", strProcessName + " Email Sending Process Started", strGUID, strServiceType, appSettings);

				dsPath = new DataSet();

				//dsPath = GetEmailTemplate(strProjectName, strProcessSubType, DI);

				//strMokshaID = DAL.ProjectSetting_Fetch("MOKSHA_SMTP", "MOKSHA_USERNAME", appSettings);
				//strMokshaPass = DAL.ProjectSetting_Fetch("MOKSHA_SMTP", "MOKSHA_PASSWORD", appSettings);

				strEmailID = strEmailids;


				if (!string.IsNullOrEmpty(strMailPath))
				{
                    strMailContent = strMailPath;//dsPath.Tables[0].Rows[0]["EMAIL_BODY_PATH"].ToString();
					strMailContent = File.ReadAllText(strMailContent);

					if (strMailType == "FILE_UPLOAD")
					{
                        strMailContent = strMailContent.Replace("$UserName$", UserName);
                        strMailContent = strMailContent.Replace("$Date$", DateTime.Now.ToString("dd-MM-yyyy"));
					}

                    else if (strMailType == "LCR_CALCULATION")
                    {
          
                        strMailContent = strMailContent.Replace("$Date$", strDate);
                    }

                    //else if (strMailType == "DAILY_STATUS")
                    //{
                    //	if (dtRecords != null && dtRecords.Rows.Count > 0)
                    //	{
                    //		strTable += EmailBody(dtRecords, strMailContent);
                    //		strMailContent = strTable;
                    //	}
                    //}

                    else if (strMailType == "FILE_SUMMARY")
                    {
                        
                    }


                }

                MailMessage Message = new MailMessage();

				SmtpClient smtpClient = new SmtpClient("smtp.office365.com");
				smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
				smtpClient.TargetName = "STARTTLS/smtp.office365.com";

				Message.From = new MailAddress(strMokshaID);

				string[] arrTo = strEmailID.Split(",");
				for (int i = 0; i < arrTo.Length; i++)
				{
					Message.To.Add(new MailAddress(arrTo[i]));
				}

				Message.Subject = strMailSubj;
				Message.IsBodyHtml = true;
				Message.Body = strMailContent;
				Message.BodyEncoding = Encoding.UTF8;
				Message.SubjectEncoding = Encoding.UTF8;

				smtpClient.Host = "smtp.office365.com";
				smtpClient.Port = 587;
				smtpClient.Credentials = new System.Net.NetworkCredential(strMokshaID, strMokshaPass);
				smtpClient.EnableSsl = true;

                #region OLD CODE
                /*
                MailMessage Message = new MailMessage();

                Message.From = new MailAddress(strMokshaID);

                string[] arrTo = strEmailID.Split(",");
                for (int i = 0; i < arrTo.Length; i++)
                {
                    Message.To.Add(new MailAddress(arrTo[i]));
                }

                Message.Subject = strMailSubj;
                Message.IsBodyHtml = true;
                Message.Body = strMailContent;
                */
                #endregion

                if (FilePath != null)
                {
                    foreach (string currFile in FilePath)
                    {
                        if (File.Exists(currFile))
                        {
                            Message.Attachments.Add(new Attachment(currFile));
                        }
                    }
                }

                smtpClient.Send(Message);

                if (Message.Attachments.Count > 0)
                {
                    foreach (Attachment att in Message.Attachments)
                    {
                        att.Dispose();
                    }
                }

                if (FilePath != null)
                {
                    Message.Attachments.Dispose();
                    foreach (string currFile in FilePath)
                    {
                        if (File.Exists(currFile))
                        {
                            File.Delete(currFile);
                        }
                    }
                }

                //DAL.WriteLog(strProcessName + " Email Sending Process Completed", appSettings, strServiceType);
                //DAL.ProcessLogInsert(strProcessName + " Email Process", "", "C", strProcessName + " Email Sending Process Completed", strGUID, strServiceType, appSettings);
            }
			catch (Exception ex)
			{
				//DAL.WriteLog("Error Occured While " + strProcessName + " Email Sending Process. Exception ==> " + ex.ToString(), appSettings, strServiceType);
				//DAL.ProcessLogInsert(strProcessName + " Email Process", "", "E", "Error Occured While " + strProcessName + " Email Sending Process. Exception ==> " + ex.ToString(), strGUID, strServiceType, appSettings);
			}
		}

		public string EmailBody(DataTable dtProcess, string strMailContent)
		{
			string EmailBody = strMailContent;
			//EmailBody = EmailBody.Replace("$var1$", "Card Dispatch Process");
			//EmailBody = EmailBody.Replace("$var2$", DateTime.Now.ToString("dd/MM/yyyy"));
			// EmailBody = EmailBody.Replace("$varProcessName$", "Auto Card Renewal Process");
			EmailBody = EmailBody.Replace("$date$", DateTime.Now.ToString("dd/MM/yy"));
			string Table = "<table border='1px' cellpadding='5' cellspacing='0' >";
			string Thead = "<thead><tr>";
			for (int i = 0; i < dtProcess.Columns.Count; i++)
			{
				Thead += "<th>" + dtProcess.Columns[i].ColumnName + "</th>";
			}
			Thead += "</tr></thead>";
			string TBody = "<tbody>";
			for (int iRow = 0; iRow < dtProcess.Rows.Count; iRow++)
			{
				string TR = "<tr>";
				for (int iCol = 0; iCol < dtProcess.Columns.Count; iCol++)
				{
					TR += "<td>" + dtProcess.Rows[iRow][iCol].ToString() + "</td>";
				}
				TR += "</tr>";
				TBody += TR;
			}
			Table = Table + Thead + TBody + "</table>";
			EmailBody = EmailBody.Replace("$Table$", Table);


			return EmailBody;
		}


		//Added For LCR
		public static void SendEmail_TFG(DataTable dtRecords, string strProjectName, string strProcessSubType, string strEmailID,
		 string strMailSubj, string strMailType,string RequestFrom,string strFromMail,string strMailPath,string strAPIUrl, string UserName, string strDate, List<string> FilePath, DependancyInjection DI)
		{
			#region
			DataSet dsRecords = new DataSet();
			DataSet dsPath = new DataSet();

			string strGUID = Guid.NewGuid().ToString();
			string strTable = "";
			string strUserID = "";
			string strPassword = "";
			string strSubID = "";
			string strDecode = "";
			string strMSGType = "";
			string strCType = "";
			string strTempName = "";
			string strURL = "";
			string strMailContent = "";
			string strCCMail = "";
			string strBCCMail = "";

			string[] tmpTo = { };
			string[] CCTo = { };
			string[] BCCTo = { };

			#endregion
			try
			{
				//DAL.ProcessLogInsert(strProcessName + " Email Process", "", "P", strProcessName + " Email Sending Process Started", strGUID, strServiceType, appSettings);
				//DAL.WriteLog(strProcessName + " Email Sending Process Started", appSettings, strServiceType);

				dsRecords = Get_LCR_ACL_Details(DI, RequestFrom);

				if (dsRecords != null && dsRecords.Tables.Count > 0 && dsRecords.Tables[0].Rows.Count > 0)
				{
					strUserID = dsRecords.Tables[0].Rows[0]["USERID"].ToString();
					strPassword = dsRecords.Tables[0].Rows[0]["PASSWORD"].ToString();
					strSubID = dsRecords.Tables[0].Rows[0]["SUBID"].ToString();
					strDecode = dsRecords.Tables[0].Rows[0]["DCODE"].ToString();
					strMSGType = dsRecords.Tables[0].Rows[0]["MSGTYPE"].ToString();
					strCType = dsRecords.Tables[0].Rows[0]["CTYPE"].ToString();
					strTempName = dsRecords.Tables[0].Rows[0]["TEMPNAME"].ToString();
					strURL = dsRecords.Tables[0].Rows[0]["URL"].ToString();
				}

				//dsPath = GetEmailTemplate(strProjectName, strProcessSubType,);

				if (!string.IsNullOrEmpty(strMailPath))
				{
					strMailContent = File.ReadAllText(strMailPath);
					string FromMail = strFromMail;
					string strToEmail = strEmailID;

                    if (strMailType == "SUMMARY_MAIL")
                    {
                        //strMailContent = strMailContent.Replace("$var1$", "Card Renewal");
                        //strMailContent = strMailContent.Replace("$var2$", DateTime.Now.ToString("dd-MM-yyyy") + " " + strNextTime);
                    }
                    if (strMailType == "FILE_UPLOAD")
                    {
                        strMailContent = strMailContent.Replace("$UserName$", UserName);
                        strMailContent = strMailContent.Replace("$Date$", DateTime.Now.ToString("dd-MM-yyyy"));
                   
                    }

                    else if (strMailType == "LCR_CALCULATION")
                    {
                        strMailContent = strMailContent.Replace("$Date$", DateTime.Now.ToString("dd-MM-yyyy"));
                    }

                    if (strToEmail != "")
					{
						List<To> lstTo = new List<To>();
						tmpTo = strToEmail.Split(',');
						for (int i = 0; i < tmpTo.Length; i++)
						{
							lstTo.Add(new To(tmpTo[i]));
						}
					}
					else
					{
						tmpTo = new string[] { };
					}

					if (strCCMail != "")
					{
						List<CC> mstTo = new List<CC>();
						CCTo = strCCMail.Split(',');
						for (int i = 0; i < CCTo.Length; i++)
						{
							mstTo.Add(new CC(CCTo[i]));
						}
					}
					else
					{
						CCTo = new string[] { };
					}

					if (strBCCMail != "")
					{
						List<BCC> kstTo = new List<BCC>();
						BCCTo = strBCCMail.Split(',');
						for (int i = 0; i < BCCTo.Length; i++)
						{
							kstTo.Add(new BCC(BCCTo[i]));
						}
					}
					else
					{
						BCCTo = new string[] { };
					}


                    List<ACL_Attachments> attachmentsList = new List<ACL_Attachments>();
                    if (FilePath != null && FilePath.Count > 0)
                    {
                        foreach (string currFile in FilePath)
                        {
                            if (File.Exists(currFile))
                            {
                                byte[] fileBytes = File.ReadAllBytes(currFile);
                                string base64Content = Convert.ToBase64String(fileBytes);

                                attachmentsList.Add(new ACL_Attachments(
                                    _content: base64Content,
                                    _filename: Path.GetFileName(currFile),
                                    _disposition: "attachment",
                                    _content_id: Guid.NewGuid().ToString(),
                                    _type: "application/octet-stream" 
                                ));
                            }
                        }
                    }

                    string[] emptyVar = new string[] { };

					Email_Json email_obj = new Email_Json();

					email_obj.userid = strUserID;
					email_obj.pwd = strPassword;
					email_obj.subuid = strSubID;
					email_obj.dcode = strDecode;
					email_obj.to = tmpTo;
					email_obj.cc = CCTo;
					email_obj.bcc = BCCTo;
					email_obj.msgtype = strMSGType;
					email_obj.from = FromMail;
					email_obj.subjectline = strMailSubj;
					email_obj.ctype = strCType;
					email_obj.msgtxt = strMailContent;
					email_obj.tempname = strTempName;

					email_obj.attachments = attachmentsList.ToArray();
                    email_obj.variables = emptyVar.ToArray();

					string jsonString_email_obj = JsonConvert.SerializeObject(email_obj);

					var requestemail = new
					{
						email = new
						{
							requestType = RequestFrom,
							Mailbody = JsonConvert.DeserializeObject(jsonString_email_obj),
							Api_Caller = strProjectName
						}
					};

					string jsonstring1 = JsonConvert.SerializeObject(requestemail);

                    //DAL.WriteLog("Mail Request==> " + jsonstring1, appSettings, strServiceType);

                    string APIURL = strAPIUrl;/*DAL.ProjectSetting_Fetch("SENDEMAIL_API", "ACL_EMAILAPI_URL", appSettings)*/;
					byte[] requestInFormOfBytes = Encoding.ASCII.GetBytes(jsonstring1);

					var httpWebRequest = (HttpWebRequest)WebRequest.Create(APIURL);
					httpWebRequest.ContentType = "application/json";
					httpWebRequest.Method = "POST";
					httpWebRequest.ContentLength = requestInFormOfBytes.Length;

					Stream webrequestStream = httpWebRequest.GetRequestStream();
					webrequestStream.Write(requestInFormOfBytes, 0, requestInFormOfBytes.Length);
					webrequestStream.Close();

					var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
					Stream responseStream = httpResponse.GetResponseStream();
					StreamReader streader = new StreamReader(responseStream, Encoding.UTF8);
					string strResult = streader.ReadToEnd();


                    if (FilePath != null)
                    {
                        foreach (string currFile in FilePath)
                        {
                            if (File.Exists(currFile))
                                File.Delete(currFile);
                        }
                    }

                    //DAL.WriteLog("Mail Reponse Daily status mail: " + strResult, appSettings, strServiceType);

                    //DAL.WriteLog(strProcessName + " Email Sending Process Completed", appSettings, strServiceType);
                    //DAL.ProcessLogInsert(strProcessName + " Email Process", "", "C", strProcessName + " Email Sending Process Completed", strGUID, strServiceType, appSettings);
                }
			}
			catch (Exception ex)
			{
                throw;
				//DAL.WriteLog("Error Occured While " + strProcessName + " Email Sending Process. Exception ==> " + ex.ToString(), appSettings, strServiceType);
				//DAL.ProcessLogInsert(strProcessName + " Email Process", "", "E", "Error Occured While " + strProcessName + " Email Sending Process. Exception ==> " + ex.ToString(), strGUID, strServiceType, appSettings);
			}
		}

		public static DataSet GetEmailTemplate(string ProcessType, string ProcessSubType, DependancyInjection DI)
		{
			DataSet dsTemplate = new DataSet();
			List<OracleParameter> lstParams = new List<OracleParameter>();

			lstParams.Add(new OracleParameter("P_PROCESS_TYPE", OracleDbType.Varchar2, ProcessType, ParameterDirection.Input));
			lstParams.Add(new OracleParameter("P_PROCESS_SUB_TYPE", OracleDbType.Varchar2, ProcessSubType, ParameterDirection.Input));
			lstParams.Add(new OracleParameter("P_RESULTSET", OracleDbType.RefCursor, null, ParameterDirection.Output));

			dsTemplate = DI.dBAccess.ExecuteDataSet("USP_GET_EMAIL_PATH", lstParams);
			return dsTemplate;
		}


		public static DataSet Get_ACL_Details(DependancyInjection DI, string strType)
        {
            try
            {
                List<OracleParameter> lstParam = new List<OracleParameter>();
                lstParam.Add(new OracleParameter("P_TYPE", OracleDbType.Varchar2, strType, ParameterDirection.Input));
                lstParam.Add(new OracleParameter("O_OUTPUT", OracleDbType.RefCursor, null, ParameterDirection.Output));
                DataSet ds = DI.dBAccess.ExecuteDataSet("USP_BOB_GET_NFSAUTOREVESAL_EMAIL_DETAIL", lstParam);
                return ds;
            }
            catch (Exception ex)
            {
                FormsAuthentication.LogException(ex, request, DI.session, "NFSAutoRevesal", "FileUpload", DI.dBAccess);
                throw;
            }
        }



		public static DataSet Get_LCR_ACL_Details(DependancyInjection DI, string strType)
		{
			try
			{
				List<OracleParameter> lstParam = new List<OracleParameter>();
				lstParam.Add(new OracleParameter("P_TYPE", OracleDbType.Varchar2, strType, ParameterDirection.Input));
				lstParam.Add(new OracleParameter("O_OUTPUT", OracleDbType.RefCursor, null, ParameterDirection.Output));
				DataSet ds = DI.dBAccess.ExecuteDataSet("USP_BOB_GET_ACL_DETAIL", lstParam);
				return ds;
			}
			catch (Exception ex)
			{
				FormsAuthentication.LogException(ex, request, DI.session, "LCR", "FileUpload", DI.dBAccess);
				throw;
			}
		}

		public static string EmailBody(DataTable dtProcess, string strMailContent, DependancyInjection DI)
        {
            try
            {
                string EmailBody = strMailContent;
                //EmailBody = EmailBody.Replace("$var1$", "Card Dispatch Process");
                //EmailBody = EmailBody.Replace("$var2$", DateTime.Now.ToString("dd/MM/yyyy"));
                // EmailBody = EmailBody.Replace("$varProcessName$", "Auto Card Renewal Process");
                EmailBody = EmailBody.Replace("$Today$", DateTime.Now.ToString("dd/MM/yy") + " ");
                string Table = "<table border='1px' cellpadding='5' cellspacing='0' >";
                string Thead = "<thead><tr>";
                for (int i = 0; i < dtProcess.Columns.Count; i++)
                {
                    Thead += "<th>" + dtProcess.Columns[i].ColumnName + "</th>";
                }
                Thead += "</tr></thead>";
                string TBody = "<tbody>";
                for (int iRow = 0; iRow < dtProcess.Rows.Count; iRow++)
                {
                    string TR = "<tr>";
                    for (int iCol = 0; iCol < dtProcess.Columns.Count; iCol++)
                    {
                        TR += "<td>" + dtProcess.Rows[iRow][iCol].ToString() + "</td>";
                    }
                    TR += "</tr>";
                    TBody += TR;
                }
                Table = Table + Thead + TBody + "</table>";
                EmailBody = EmailBody.Replace("$Table$", Table);


                return EmailBody;
            }
            catch (Exception ex)
            {
                FormsAuthentication.LogException(ex, request, DI.session, "NFSAutoRevesal", "FileUpload", DI.dBAccess);
                throw;
            }
        }


        //ReportConfiguration By Filter
        public DataSet GetReportConfigForGrid(string strSearchValue, string strSearchColumn,
          string strOrderByColumn, string strOrderByType, int iPageNo, int iPageNoOfRows, string strEntityName, string strReportName, string strTemplateName, string ViewName, DependancyInjection DI)
        {
            List<OracleParameter> commands = new List<OracleParameter>();

            commands.Add(new OracleParameter("p_viewname", OracleDbType.Varchar2, ViewName, System.Data.ParameterDirection.Input));
            commands.Add(new OracleParameter("p_SearchColumn", OracleDbType.Varchar2, strSearchColumn, System.Data.ParameterDirection.Input));
            commands.Add(new OracleParameter("p_SearchValue", OracleDbType.Varchar2, strSearchValue, System.Data.ParameterDirection.Input));
            commands.Add(new OracleParameter("p_OrderByCol", OracleDbType.Varchar2, strOrderByColumn, System.Data.ParameterDirection.Input));
            commands.Add(new OracleParameter("p_OrderByType", OracleDbType.Varchar2, strOrderByType, System.Data.ParameterDirection.Input));
            commands.Add(new OracleParameter("p_PageNo", OracleDbType.Varchar2, iPageNo, System.Data.ParameterDirection.Input));
            commands.Add(new OracleParameter("p_NoOfRows", OracleDbType.Varchar2, iPageNoOfRows, System.Data.ParameterDirection.Input));
            commands.Add(new OracleParameter("p_EntityName", OracleDbType.Varchar2, iPageNoOfRows, System.Data.ParameterDirection.Input));
            commands.Add(new OracleParameter("p_ReportName", OracleDbType.Varchar2, iPageNoOfRows, System.Data.ParameterDirection.Input));
            commands.Add(new OracleParameter("p_TemplateName", OracleDbType.Varchar2, iPageNoOfRows, System.Data.ParameterDirection.Input));
            commands.Add(new OracleParameter("v_CursorToalRecords", OracleDbType.RefCursor, null, System.Data.ParameterDirection.Output));
            commands.Add(new OracleParameter("v_CursorFetchRecords", OracleDbType.RefCursor, null, System.Data.ParameterDirection.Output));

            DataSet ds = DI.dBAccess.ExecuteDataSet("USP_AKR_REPORTCONFIG_FILTER", commands);
            return ds;
        }
    }
}
