using MasterApplication.Areas.Admin.Models;
using MasterApplication.DAL;
using MasterApplication.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.DirectoryServices.Protocols;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace MasterApplication.Areas.Admin.Controllers
{
	//24062025
	[Area("Admin")]
	public class LoginController : Controller
	{
		readonly DependancyInjection DI;
		public LoginController(DependancyInjection _dependancyInjection)
		{
			DI = _dependancyInjection;
		}
		public IActionResult Index()
		{
			return View();
		}
		//[Area("Admin")]
		public IActionResult Login()
		{
			return View();
		}
		//public IActionResult LoginUser()
		//{
		//    return RedirectToAction("Index");
		//}
		//[Area("Admin")]
		[HttpPost]

		public IActionResult LoginUser([Bind] LoginModel LM)
		{
			bool IsUserValidated = false;
			string strVal = "";
			// ViewBag.LoginViewData = "Kindly Login to access portal "+LM.TXT_UserName;
			//if (LM.TXT_UserName != null && LM.TXT_Password != null)
			if (LM.TXT_UserName == null && LM.TXT_Password == null)
			{
				TempData["Message"] = "nullValues|Please fill mandatory fields!";
				return View("Index");
			}
			else
			{
				LM.TXT_UserName = MSEncrypto.Encryption.Decrypt(LM.TXT_UserName);
				bool isAdUser = false;
				string strLocked = "0";
				(isAdUser, strLocked) = Check_User_Type(LM.TXT_UserName, LM.TXT_Password);

				if (strLocked == "1")
				{
					strVal = "LOCKED";
				}
				else
				{
					if (isAdUser)
					{
						//string strPassword = MSEncrypto.Encryption.Decrypt(LM.TXT_Password);
						IsUserValidated = Authenticate_AD_User(LM.TXT_UserName, LM.TXT_Password, DI.myAppSettings.LDAP_DOMAIN_NAME);

						IsUserValidated = true;

                        if (IsUserValidated == true)
						{
							DataSet dtSet = LoginAttempt(LM.TXT_UserName, "VALID", "");
							strVal = "true";
						}
						else
						{
							DataSet dtSet = LoginAttempt(LM.TXT_UserName, "INVALID", "");

							if (dtSet != null && dtSet.Tables.Count > 0 && dtSet.Tables[0].Rows.Count > 0)

							{

								int LoginCount = Convert.ToInt32(dtSet.Tables[0].Rows[0]["COUNTER"].ToString());

								if (LoginCount > 3)
								{
									ViewData["ErrorMessage"] = "Maximum Attempt Reached. Enter Valid UserName and Password";

									TempData["Message"] = "errorLimit|Maximum Attempt Reached. Enter Valid UserName and Password";

									strVal = "LOCKED";
								}
							}

							strVal = "false";

						}
					}
					else
					{
						//IsUserValidated = Authenticate_APP_User(LM.TXT_UserName, LM.TXT_Password);
						strVal = Authenticate_APP_User(LM.TXT_UserName, LM.TXT_Password);

					}
				}
				//if (IsUserValidated)
				//{
				//    Update_ActiveUser(LM.TXT_UserName, LM.TXT_Password, isAdUser);
				//    return RedirectToAction("Index", "Home", new { area = "Admin" });
				//}
				/*
                if (IsUserValidated)
                {
                    bool loginChk = LoginChk(LM.TXT_UserName);
                    if (loginChk)
                    {
                        Update_ActiveUser(LM.TXT_UserName, LM.TXT_Password, isAdUser);
                        TempData["Message"] = "LoginChk";
                    }
                    else
                    {
                        Update_ActiveUser(LM.TXT_UserName, LM.TXT_Password, isAdUser);
                        return RedirectToAction("Index", "Home", new { area = "Admin" });
                    }
                }
                else
                {
                    TempData["Message"] = "errorInvalid|UserName or Password is incorrect!";
                    //ViewData["ErrorMessage"] = "UserName or Password is incorrect!";
                    return View("Login");
                    //return RedirectToAction("Index", "Home", new { area = "Admin" });
                }
                */
				if (strVal.ToUpper() == "TRUE")
				{
					bool loginChk = LoginChk(LM.TXT_UserName);
					if (loginChk)
					{
						ActiveUser av = Store_ActiveUser(LM.TXT_UserName, LM.TXT_Password, isAdUser);

						TempData["ActiveUser"] = JsonConvert.SerializeObject(av);
						TempData["Message"] = "LoginChk";
						//return RedirectToAction("Store_ActiveUser", "Login", new { area = "Admin" });
					}
					else
					{
						Update_ActiveUser(LM.TXT_UserName, LM.TXT_Password, isAdUser);
						return RedirectToAction("Index", "Home", new { area = "Admin" });
					}
				}
				else if (strVal.ToUpper() == "LOCKED")
				{
					//ViewData["ErrorMessage"] = "Maximum Attempt Reached. Enter Valid UserName and Password";
					ViewData["ErrorMessage"] = "UserName is locked, Kindly contact Administrator";

					TempData["Message"] = "errorLimit|UserName is locked, Kindly contact Administrator";

					return View("Index");
				}
				else
				{
					TempData["Message"] = "errorInvalid|UserName or Password is incorrect!";
					//ViewData["ErrorMessage"] = "UserName or Password is incorrect!";
					return View("Index");
					//return RedirectToAction("Index", "Home", new { area = "Admin" });
				}
			}
			//TempData["Message"] = "nullValues|Please fill mandatory fields!";
			return View("Index");

		}
		public (bool, string) Check_User_Type(string username, string strPassword)
		{
			List<OracleParameter> commands = new List<OracleParameter>();

			commands.Add(new OracleParameter("p_UserName", OracleDbType.Varchar2, username, System.Data.ParameterDirection.Input));
			//commands.Add(new OracleParameter("p_UserName", OracleDbType.Varchar2, strPassword, System.Data.ParameterDirection.Input));
			commands.Add(new OracleParameter("v_cursor2", OracleDbType.RefCursor, null, System.Data.ParameterDirection.Output));

			DataSet dtResultFetchRecord = DI.dBAccess.ExecuteDataSet_ADM("USP_BOB_ADM_FETCHUSERRECORD", commands);
			if (dtResultFetchRecord.Tables[0].Rows.Count == 0)
			{
				return (false, "0");
			}
			//DataSet dtResultFetchRecord = dBAccess.ExecuteDataSet("BYT_usp_FetchUserRecord", commands);
			bool isAdUser = Convert.ToBoolean(dtResultFetchRecord.Tables[0].Rows[0][0]);
			string strLocked = dtResultFetchRecord.Tables[0].Rows[0]["locked"].ToString();

			if (!isAdUser)
			{
				List<OracleParameter> lstPara = new List<OracleParameter>();

				lstPara.Add(new OracleParameter("p_UserName", OracleDbType.Varchar2, username, System.Data.ParameterDirection.Input));
				lstPara.Add(new OracleParameter("p_UserName", OracleDbType.Varchar2, strPassword, System.Data.ParameterDirection.Input));
				lstPara.Add(new OracleParameter("v_cursor2", OracleDbType.RefCursor, null, System.Data.ParameterDirection.Output));

				DataSet dtStatus = DI.dBAccess.ExecuteDataSet_ADM("USP_BOB_ADM_UNLOCK_APP_USER", lstPara);

				strLocked = dtStatus.Tables[0].Rows[0]["locked"].ToString();
			}

			return (isAdUser, strLocked);
		}

		public bool Authenticate_AD_User(string UserName, string Password, string LDAP_Domain_Name)
		{
			Password = MSEncrypto.Encryption.Decrypt(Password);
			try
			{
				LdapConnection lcon = null;
				NetworkCredential nc = null;
				if (LDAP_Domain_Name == "")
				{
					lcon = new LdapConnection
					(new LdapDirectoryIdentifier((string)null, false, false));
					nc = new NetworkCredential(UserName, Password, Environment.UserDomainName);
				}
				else
				{
					lcon = new LdapConnection
					(new LdapDirectoryIdentifier((string)null, false, false));
					nc = new NetworkCredential(UserName,
										   Password, LDAP_Domain_Name);
				}

				lcon.Credential = nc;
				lcon.AuthType = AuthType.Negotiate;
				// user has authenticated at this point,
				// as the credentials were used to login to the dc.
				lcon.Bind(nc);
				return true;
			}
			catch (Exception)
			{
				ViewData["ErrorMessage"] = "UserName or Password is incorrect!";
				return false;
			}
		}

		public string Authenticate_APP_User(string UserName, string Password)
		{
			string strVal = "";
			try
			{
				List<OracleParameter> commands = new List<OracleParameter>();
				commands.Add(new OracleParameter("p_UserName", OracleDbType.Varchar2, UserName, System.Data.ParameterDirection.Input));
				//commands.Add(new OracleParameter("p_Password", OracleDbType.Varchar2, MSEncrypto.Encryption.Encrypt(Password), System.Data.ParameterDirection.Input));
				commands.Add(new OracleParameter("p_Password", OracleDbType.Varchar2, Password, System.Data.ParameterDirection.Input));
				commands.Add(new OracleParameter("v_cursor", OracleDbType.RefCursor, null, System.Data.ParameterDirection.Output));

				DataSet dataSet = DI.dBAccess.ExecuteDataSet_ADM("USP_BOB_ADM_GETUSERINFO", commands);
				//DataSet dataSet = dBAccess.ExecuteDataSet("BYT_USP_GETUSERINFO", commands);
				if (dataSet.Tables[0].Rows.Count > 0 && dataSet.Tables[0].Rows[0][0].ToString().ToUpper() == "LOCKED")
				{
					ViewData["ErrorMessage"] = "UserName is locked, Kindly contact Administrator";
					return strVal = "LOCKED";
				}
				else if (dataSet.Tables[0].Rows.Count > 0)
				{
					DataSet dtSet = LoginAttempt(UserName, "VALID", Password);
					return strVal = "true";
				}
				else
				{
					DataSet dtSet = LoginAttempt(UserName, "INVALID", Password);

					if (dtSet != null && dtSet.Tables.Count > 0 && dtSet.Tables[0].Rows.Count > 0)

					{

						int LoginCount = Convert.ToInt32(dtSet.Tables[0].Rows[0]["COUNTER"].ToString());

						if (LoginCount > 3)
						{
							ViewData["ErrorMessage"] = "Maximum Attempt Reached. Enter Valid UserName and Password";

							TempData["Message"] = "errorLimit|Maximum Attempt Reached. Enter Valid UserName and Password";

							return strVal = "LOCKED";
						}
						/*
                        else
                        {
                            ViewData["ErrorMessage"] = "UserName or Password is incorrect!";

                            TempData["Message"] = "errorInvalid|UserName or Password is incorrect!";
                        }
                        */
					}

					return strVal = "false";

					//ViewData["ErrorMessage"] = "UserName or Password is incorrect!";
					//return false;
				}

			}
			catch (Exception)
			{
				//return false;                //return false;
				return strVal = "false";
			}
		}
		public ActiveUser Store_ActiveUser(string UserName, string Password, bool IsADUser)
		{
			string ProcedureName = "";
			List<OracleParameter> commands = new List<OracleParameter>();
			DataSet dsResult = new DataSet();
			commands.Add(new OracleParameter("p_UserName", OracleDbType.Varchar2, UserName, System.Data.ParameterDirection.Input));

			if (IsADUser == true)
			{
				//ProcedureName = "BYT_USP_GETUSERINFOFORWINAUTH";
				ProcedureName = "USP_BOB_ADM_GETUSERINFOFORWINAUTH";
			}
			else
			{
				commands.Add(new OracleParameter("p_Password", OracleDbType.Varchar2, Password, System.Data.ParameterDirection.Input));
				//commands.Add(new OracleParameter("p_Password", OracleDbType.Varchar2, MSEncrypto.Encryption.Encrypt(Password), System.Data.ParameterDirection.Input));

				//ProcedureName = "BYT_USP_GETUSERINFO";
				ProcedureName = "USP_BOB_ADM_GETUSERINFO";
			}
			commands.Add(new OracleParameter("v_cursor", OracleDbType.RefCursor, null, System.Data.ParameterDirection.Output));
			dsResult = DI.dBAccess.ExecuteDataSet_ADM(ProcedureName, commands);
			DataTable dtResult = dsResult.Tables[0];
			ActiveUser av = FormsAuthentication.ForceLoginInfo(HttpContext, DI.session, Convert.ToInt16(dtResult.Rows[0]["Code"]), UserName.ToString(),
				//MSEncrypto.Encryption.Encrypt(Password).ToString()
				Password.ToString()
						   , dtResult.Rows[0]["LastLoginDateTime"].ToString(), dtResult.Rows[0]["ExpiryDateTime"].ToString(),
						   Convert.ToBoolean((dtResult.Rows[0]["IsADUser"])), Convert.ToBoolean((dtResult.Rows[0]["IsLocked"])),
							dtResult.Rows[0]["ImageName"].ToString(), dtResult.Rows[0]["LoginTime"].ToString(),
						   Convert.ToBoolean((dtResult.Rows[0]["IsAdmin"])), Convert.ToBoolean((dtResult.Rows[0]["IsEdit"])), Convert.ToBoolean((dtResult.Rows[0]["IsDelete"]))
						 , DI.dBAccess);
			return av;
		}

		public void Update_ActiveUser(string UserName, string Password, bool IsADUser)
		{
			string ProcedureName = "";
			List<OracleParameter> commands = new List<OracleParameter>();
			DataSet dsResult = new DataSet();
			commands.Add(new OracleParameter("p_UserName", OracleDbType.Varchar2, UserName, System.Data.ParameterDirection.Input));

			if (IsADUser == true)
			{
				//ProcedureName = "BYT_USP_GETUSERINFOFORWINAUTH";
				ProcedureName = "USP_BOB_ADM_GETUSERINFOFORWINAUTH";
			}
			else
			{
				commands.Add(new OracleParameter("p_Password", OracleDbType.Varchar2, Password, System.Data.ParameterDirection.Input));
				//commands.Add(new OracleParameter("p_Password", OracleDbType.Varchar2, MSEncrypto.Encryption.Encrypt(Password), System.Data.ParameterDirection.Input));

				//ProcedureName = "BYT_USP_GETUSERINFO";
				ProcedureName = "USP_BOB_ADM_GETUSERINFO";
			}
			commands.Add(new OracleParameter("v_cursor", OracleDbType.RefCursor, null, System.Data.ParameterDirection.Output));
			dsResult = DI.dBAccess.ExecuteDataSet_ADM(ProcedureName, commands);
			DataTable dtResult = dsResult.Tables[0];
			FormsAuthentication.NormalLoginUpdate(HttpContext, DI.session, Convert.ToInt16(dtResult.Rows[0]["Code"]), UserName.ToString(),
				//MSEncrypto.Encryption.Encrypt(Password).ToString()
				Password.ToString()
						   , dtResult.Rows[0]["LastLoginDateTime"].ToString(), dtResult.Rows[0]["ExpiryDateTime"].ToString(),
						   Convert.ToBoolean((dtResult.Rows[0]["IsADUser"])), Convert.ToBoolean((dtResult.Rows[0]["IsLocked"])),
							dtResult.Rows[0]["ImageName"].ToString(), dtResult.Rows[0]["LoginTime"].ToString(),
						   Convert.ToBoolean((dtResult.Rows[0]["IsAdmin"])), Convert.ToBoolean((dtResult.Rows[0]["IsEdit"])), Convert.ToBoolean((dtResult.Rows[0]["IsDelete"]))
						 , DI.dBAccess);

		}

		public IActionResult Logout()
		{
			ActiveUser av = FormsAuthentication.GetCurrentUser(DI.session);
			if (av != null)
			{
				FormsAuthentication.UpdateUserActivityLog(DI.session, "Logout", false, true, true, DI.dBAccess);
				DI.session.Clear();
				if (Request.Cookies[".AspNetCore.Session"] != null)
				{
					Response.Cookies.Delete(".AspNetCore.Session");
				}

			}

			return Redirect(DI.myAppSettings.LoginURL);

			//return RedirectToAction("Index", "Login", new { area = "Admin" });
		}
		public JsonResult EnctryptSting(string Pass, string UserName)
		{
			//LoginModel model = new LoginModel();
			try
			{
				if (Pass != null)
				{
					Pass = (MSEncrypto.Encryption.Encrypt(Pass));
				}

				if (UserName != null)
				{
					UserName = (MSEncrypto.Encryption.Encrypt(UserName));
				}
				//Pass = (MSEncrypto.Encryption.Encrypt(Pass));
				//model.TXT_Password = Pass;
			}
			catch (Exception ex)
			{

			}
			return Json(new { Password = Pass, Username = UserName });
		}
		public bool LoginChk(string userName)
		{
			bool status = false;

			List<OracleParameter> commands = new List<OracleParameter>();
			commands.Add(new OracleParameter("p_UserName", OracleDbType.Varchar2, userName, System.Data.ParameterDirection.Input));
			commands.Add(new OracleParameter("v_cursor", OracleDbType.RefCursor, null, System.Data.ParameterDirection.Output));
			DataSet dtResultFetchRecord = DI.dBAccess.ExecuteDataSet_ADM("BOB_ADM_CHK_LOGIN", commands);
			if (dtResultFetchRecord != null && dtResultFetchRecord.Tables.Count > 0 && dtResultFetchRecord.Tables[0].Rows.Count > 0)
			{
				status = true;
			}
			return status;
		}
		public IActionResult ForceLogin([Bind] LoginModel LM)
		{
			ActiveUser av = JsonConvert.DeserializeObject<ActiveUser>(TempData["ActiveUser"].ToString());
			TempData.Remove("ActiveUser");
			FormsAuthentication.SaveLogin(av, DI.session, DI.dBAccess);
			return RedirectToAction("Index", "Home", new { area = "Admin" });
		}
		public DataSet LoginAttempt(string UserName, string status, string Password)
		{
			List<OracleParameter> commands = new List<OracleParameter>();
			commands.Add(new OracleParameter("p_UserName", OracleDbType.Varchar2, UserName, System.Data.ParameterDirection.Input));
			//commands.Add(new OracleParameter("p_Password", OracleDbType.Varchar2, MSEncrypto.Encryption.Encrypt(Password), System.Data.ParameterDirection.Input));
			commands.Add(new OracleParameter("p_Password", OracleDbType.Varchar2, Password, System.Data.ParameterDirection.Input));
			commands.Add(new OracleParameter("p_Status", OracleDbType.Varchar2, status, System.Data.ParameterDirection.Input));
			commands.Add(new OracleParameter("v_cursor", OracleDbType.RefCursor, null, System.Data.ParameterDirection.Output));

			DataSet dataSet = DI.dBAccess.ExecuteDataSet_ADM("USP_BOB_ADM_LOGIN_ATTEMPT", commands);
			return dataSet;
		}
		public IActionResult Store_ActiveUser()
		{
			ActiveUser av = (ActiveUser)TempData["ActiveUser"];
			FormsAuthentication.SaveLogin(av, DI.session, DI.dBAccess);
			return View("Index");
		}
	}
}
