using MasterApplication.Areas.Admin.Models;
using MasterApplication.Services;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace MasterApplication.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class SecurityQuestionsController : Controller
    {
        readonly DependancyInjection DI;

        public SecurityQuestionsController(DependancyInjection _DI)
        {
            DI = _DI;
            ViewBag.UserMenuList = DI.GetMenuList(DI.session);
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public IActionResult GetQuestions()
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
            DataSet dataSet = DI.commonClass.GetMasterForGrid_ADM(strSearchValue, strSearchColumn, strSortColumn,
                                                               strSortType, start, length, "VW_BOB_SecurityQuestions",DI);
            int recordsFiltered = Convert.ToInt16(dataSet.Tables[0].Rows[0]["TotalRecords"]);
            int TotalRecords = dataSet.Tables[1].Rows.Count;
            string json = JsonConvert.SerializeObject(dataSet.Tables[1], Formatting.Indented);
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
        public IActionResult Create([Bind] SecurityQuestionsModel ques)
        {
            ActiveUser av = FormsAuthentication.GetCurrentUser(DI.session);
            if (ModelState.IsValid)
            {
                try
                {
                    DataSet dataSet = BL.SecurityQuestions.InsertSecurityQuestions(ques.Question, DI.dBAccess);
                    if (dataSet.Tables[0].Rows.Count > 0)
                    {
                        if (dataSet.Tables[0].Rows[0][0].ToString() == "Saved")
                        {
                            TempData["Message"] = "success|SecurityQuestion added successfully";
                            return RedirectToAction("Index");
                        }
                        else
                        {
                            TempData["Message"] = "error|SecurityQuestion name already exists!";
                            return RedirectToAction("Create");
                        }
                    }
                    else
                    {
                        TempData["Message"] = "success|Error occurred while creating SecurityQuestion!";
                        return RedirectToAction("Create");
                    }

                }
                catch (Exception ex)
                {
                    FormsAuthentication.LogException(ex, Request, DI.session, "SecurityQuestions", "Create", DI.dBAccess);
                    TempData["Message"] = "error|Error occurred while creating user!";
                    return View();
                }
            }
            else
            {
                TempData["Message"] = "error|Data cannot be empty";
                return RedirectToAction("Create");
            }
        }
        public IActionResult Edit(string Code)
        {
            try
            {

                DataSet dataSet = BL.SecurityQuestions.SelectSecurityQuestions(Code, DI.dBAccess);
                List<SecurityQuestionsModel> SecurityQuestions = new List<SecurityQuestionsModel>();
                for (int i = 0; i < dataSet.Tables[0].Rows.Count; i++)
                {
                    SecurityQuestions.Add(new SecurityQuestionsModel
                    {
                        Question = dataSet.Tables[0].Rows[i]["QUESTION"].ToString(),
                        LOCKED = Convert.ToBoolean(Convert.ToInt16(dataSet.Tables[0].Rows[i]["LOCKED"])),
                        CODE = dataSet.Tables[0].Rows[i]["CODE"].ToString()
                    });
                }


                return View("Edit", SecurityQuestions);
            }
            catch (Exception ex)
            {
                FormsAuthentication.LogException(ex, Request, DI.session, "SecurityQuestions", "Edit(string code)", DI.dBAccess);
                return View();
            }
        }
        [HttpPost]
        public IActionResult Edit([Bind] SecurityQuestionsModel ques)
        {
            try
            {
                ActiveUser av = FormsAuthentication.GetCurrentUser(DI.session);
                DataSet dataSet = BL.SecurityQuestions.UpdateSecurityQuestions(ques.CODE, ques.Question, Convert.ToBoolean(ques.LOCKED), DI.dBAccess);
                TempData["Message"] = "success|SecurityQuestions updated successfully";
            }
            catch (Exception ex)
            {
                FormsAuthentication.LogException(ex, Request, DI.session, "SecurityQuestions", "Edit", DI.dBAccess);
                TempData["Message"] = "error|Error occurred while updating record";
            }
            return RedirectToAction("Index");
        }
        public IActionResult Delete(int Code)
        {
            try
            {
                DataSet dataSet = BL.SecurityQuestions.SecurityQuestionsDelete(Code, DI.dBAccess);
                if (dataSet.Tables[0].Rows.Count > 0)
                {
                    if (dataSet.Tables[0].Rows[0][0].ToString() == "Saved")
                    {
                        TempData["Message"] = "success|SecurityQuestion deleted successfully";
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        TempData["Message"] = "error|SecurityQuestion is already mapped to user!";

                    }
                }
                else
                {
                    TempData["Message"] = "success|Error occurred while creating SecurityQuestion!";

                }
            }
            catch (Exception ex)
            {
                FormsAuthentication.LogException(ex, Request, DI.session, "SecurityQuestions", "Delete", DI.dBAccess);
                TempData["Message"] = "error|Error occurred while deleting record";
            }
            return RedirectToAction("Index");
        }
    }
}
