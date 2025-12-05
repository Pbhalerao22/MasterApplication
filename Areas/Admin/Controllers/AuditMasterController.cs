using MasterApplication.Areas.Admin.Models;
using MasterApplication.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System;
using System.Data;
using MasterApplication.Areas.Admin.BL;

namespace MasterApplication.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AuditMasterController : Controller
    {

        readonly DependancyInjection DI;

        public AuditMasterController(DependancyInjection _DI)
        {
            DI = _DI;
        }
        public IActionResult Index()
        {
            try
            {

                DataSet ds_u = AuditMaster.GetListOfUsers(DI.dBAccess);

                DataSet ds = AuditMaster.GetTypeList(DI.dBAccess);

                ViewBag.Type = ds.Tables[0].Rows;

                ViewBag.UserList = ds_u.Tables[0].Rows;
                return View();
            }
            catch (Exception ex)
            {
                FormsAuthentication.LogException(ex, Request, DI.session, "AuditMaster", "Index", DI.dBAccess);
                TempData["Message"] = "error|Error occurred while rendring AuditMaster page";
                return View();
            }
        }

        [HttpGet]
        public IActionResult GetAuditDetails(string Type, string usercode, string FromDate, string ToDate, string IsDownload)
        {

            try
            {
                if (string.IsNullOrEmpty(Type))
                    return BadRequest("Type is required.");

                DateTime? from = null;
                DateTime? to = null;

                if (!string.IsNullOrEmpty(FromDate) && !string.IsNullOrEmpty(ToDate))
                {
                    if (DateTime.TryParse(FromDate, out DateTime f) && DateTime.TryParse(ToDate, out DateTime t))
                    {
                        from = f;
                        to = t;
                    }
                    else
                    {
                        return BadRequest("Invalid date range.");
                    }
                }
                int userCodeInt = Convert.ToInt32(usercode);
                DataSet ds = AuditMaster.GetAuditDetails(Type, userCodeInt, from, to, DI.dBAccess);

                if (ds == null || ds.Tables.Count == 0)
                    return Json(new { success = false, message = "No data found" });

                // ds.Tables[0] -> audit data
                // ds.Tables[1] -> filename
                if (IsDownload == "1")
                {
                    string filename = ds.Tables[1].Rows[0]["FILENAME"].ToString();


                    byte[] fileBytes = new ExcelCreate().CreateNewExcel(ds.Tables[0]);
                    return File(fileBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", filename);

                }
                else
                {
                    return Json(new { success = true, data = ds.Tables[0] });
                }
            }
            catch (Exception ex)
            {

                FormsAuthentication.LogException(ex, Request, DI.session, "AuditMaster", "GetAuditDetails", DI.dBAccess);
                TempData["Message"] = "error|Error occurred while fetching/downloading AuditMaster data";
                return Json(new { success = false, message = "Error occurred while fetching/downloading AuditMaster data" });
            }

        }
    }
}
