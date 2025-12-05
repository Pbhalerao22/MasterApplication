using MasterApplication.Areas.Admin.Models;
using MasterApplication.Services;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OfficeOpenXml;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace MasterApplication.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class SchedulerController : Controller
    {
        readonly DependancyInjection DI;
        public SchedulerController(DependancyInjection _dependancyInjection) {  DI = _dependancyInjection; }
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult GetSchedulerDetails()
        {
            string JsonString = Request.Form.Keys.FirstOrDefault();
            JObject JArray = JObject.Parse(JsonString);

            int start = Convert.ToInt16(JArray["PageNo"].ToString());
            int length = Convert.ToInt16(JArray["PageSize"].ToString());
            string strSearchColumn = JArray["SearchColumn"].ToString();
            string strSearchValue = JArray["SearchValue"].ToString();
            string strSortColumn = JArray["SortColumn"].ToString();
            string strSortType = JArray["SortType"].ToString();

            int pageSize = length != null ? Convert.ToInt32(length) : 0;
            int skip = start != null ? Convert.ToInt32(start * pageSize) : 0;
            int recordsTotal = 0;
            DataSet dataSet = DI.commonClass.GetMasterForGrid(strSearchValue, strSearchColumn, strSortColumn,
                                                                strSortType, start, length, "vw_bob_scheduler_detail", DI);


            int recordsFiltered = Convert.ToUInt16(dataSet.Tables[0].Rows[0]["TotalRecords"]);
            int TotalRecords = dataSet.Tables[1].Rows.Count;
            string json = JsonConvert.SerializeObject(dataSet.Tables[1], Newtonsoft.Json.Formatting.Indented);
            var jsonData = new
            {
                //draw = start=start+1, 
                recordsFiltered = recordsFiltered,
                recordsTotal = TotalRecords,
                data = json
                //data = dataSet.Tables[0]
            };
            return Ok(jsonData);

        }
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create([Bind] SchedulerModel Sc)
        {
            ActiveUser av = FormsAuthentication.GetCurrentUser(DI.session);

            try
            {

                DataSet dataSet = BL.Scheduler.InsertSchedular(Sc.ProjectName,Sc.PROCESS_TYPE, Sc.EXECUTION_DATE, Sc.EXECUTION_TIME,Sc.ProcessStatus, av.UserCode, DI.dBAccess);
                if (dataSet != null && dataSet.Tables[0].Rows.Count > 0)
                {
                    if (dataSet.Tables[0].Rows[0][0].ToString().ToUpper() == "SAVED")
                    {
                        TempData["Message"] = "success|Record added successfully";
                        return RedirectToAction("Index");
                    }
                    else if (dataSet.Tables[0].Rows[0][0].ToString().ToUpper() == "EXISTS")
                    {
                        TempData["Message"] = "error|Record already exists!";
                    }
                }
                else
                {
                    TempData["Message"] = "error|Error occurred while creating Record!";
                }
                return View();
            }
            catch (Exception ex)
            {
                FormsAuthentication.LogException(ex, Request, DI.session, "DigitalToPhysical/Scheduler", "Create", DI.dBAccess);
                TempData["Message"] = "error|Error occurred while creating Record!";
                return View();
            }
        }
        public IActionResult Edit(string Code)
        {
            try
            {
                DataSet dataSet = BL.Scheduler.SelectSchedular(Code, DI.dBAccess);
                List<SchedulerModel> schedular = new List<SchedulerModel>();
                for (int i = 0; i < dataSet.Tables[0].Rows.Count; i++)
                {
                    schedular.Add(new SchedulerModel
                    {
                        CODE = dataSet.Tables[0].Rows[i]["SYS_CODE"].ToString(),
                        ProjectName= dataSet.Tables[0].Rows[i]["PROJECT_NAME"].ToString(),
                        EXECUTION_DATE = dataSet.Tables[0].Rows[i]["EXECUTION_DATE"].ToString(),
                        EXECUTION_TIME = dataSet.Tables[0].Rows[i]["EXECUTION_TIME"].ToString(),
                        PROCESS_TYPE = dataSet.Tables[0].Rows[i]["PROCESS_TYPE"].ToString(),
                        EMAILIDS = dataSet.Tables[0].Rows[i]["EMAIL_ID"].ToString(),
                        ProcessStatus= dataSet.Tables[0].Rows[i]["PROCESS_STATUS"].ToString(),
                    });
                }

                return View("Edit", schedular);
            }
            catch (Exception ex)
            {
                FormsAuthentication.LogException(ex, Request, DI.session, "Scheduler", "Edit(string code)", DI.dBAccess);
                return View();
            }
        }

        [HttpPost]
        public IActionResult Edit([Bind] SchedulerModel sm)
        {
            try
            {
                ActiveUser av = FormsAuthentication.GetCurrentUser(DI.session);
                DataSet dataSet = BL.Scheduler.UpdateSchedular(sm.CODE,sm.ProjectName,sm.PROCESS_TYPE, sm.EXECUTION_DATE, sm.EXECUTION_TIME, sm.EMAILIDS,sm.ProcessStatus, DI.dBAccess);
                TempData["Message"] = "success|Record updated successfully";
            }
            catch (Exception ex)
            {
                FormsAuthentication.LogException(ex, Request, DI.session, "Scheduler", "Edit", DI.dBAccess);
                TempData["Message"] = "error|Error occurred while updating record";
            }
            return RedirectToAction("Index");
        }
    }
}
