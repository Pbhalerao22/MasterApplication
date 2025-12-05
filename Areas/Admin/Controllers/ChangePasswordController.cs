using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MasterApplication.Areas.Admin.Models;
using MasterApplication.Services;
using Oracle.ManagedDataAccess.Client;

using System.Data;


namespace MasterApplication.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ChangePasswordController : Controller
    {
        readonly DependancyInjection DI;

        public ChangePasswordController(DependancyInjection _dependancyInjection)
        {
            DI = _dependancyInjection;
        }
        public IActionResult Index()
        {
            ActiveUser av = FormsAuthentication.GetCurrentUser(DI.session);
            string UserName = av.UserName;
            ViewBag.Username = UserName;
            ChangePasswordModel changePassword = new ChangePasswordModel();
            changePassword.username = UserName;
            return View(changePassword);
        }
        [HttpPost]
        public IActionResult Index(ChangePasswordModel changePassword)
        {
            ActiveUser av = FormsAuthentication.GetCurrentUser(DI.session);

            if (ModelState.IsValid)
            {
                try
                {

                    if (changePassword.password.Length >= 6) { 
                    if (changePassword.password == changePassword.re_password)
                    {
                        changePassword.password = MSEncrypto.Encryption.Encrypt(changePassword.password);
                        DataSet ds = BL.ChangePaasword.InsertPass(changePassword.username, changePassword.password, changePassword.re_password, DI.dBAccess);

                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            if (ds.Tables[0].Rows[0][0].ToString() == "Save")
                            {
                                TempData["Message"] = "success|Password Change successfully";
                                return RedirectToAction("Index");
                            }
                            else if (ds.Tables[0].Rows[0][0].ToString() == "Error")
                            {
                                TempData["Message"] = "error|Password Does Not Match";
                                return RedirectToAction("Index");
                            }
                        }
                        else
                        {
                            return RedirectToAction("Index");
                        }
                    }
                    else
                    {
                        TempData["Message"] = "error|Password Does Not Match!";
                        return RedirectToAction("Index");
                    }
                }
                    else
                    {
                        TempData["Message"] = "error| Password Should Greater than 6 digit!";
                    }
                    return RedirectToAction("Index");
                }
                catch(Exception ex) 
                {
                    FormsAuthentication.LogException(ex, Request, DI.session, "MenuMaster", "Create", DI.dBAccess);
                    TempData["Message"] = "error|Error occurred while changing password!";
                    return RedirectToAction("Index");
                }
            }
            else
            {
                TempData["Message"] = "error|Data cannot be empty";
                return RedirectToAction("Index");
            }

        }



    }
}
