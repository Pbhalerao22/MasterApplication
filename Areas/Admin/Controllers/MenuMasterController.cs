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
    public class MenuMasterController : Controller
    {
        readonly DependancyInjection DI;
        readonly CommonClass commonClass;
        public MenuMasterController(DependancyInjection _dependancyInjection, CommonClass _commonClass)
        {
            DI = _dependancyInjection;
            commonClass = _commonClass;
        }
        //[Area("Admin")]
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public IActionResult GetMenus()
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
                                                               strSortType, start, length, "VW_BOB_MenuMaster", DI);
            int recordsFiltered = Convert.ToUInt16(dataSet.Tables[0].Rows[0]["TotalRecords"]);
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

        public IActionResult MenuCreate()
        {
            ViewBag.ParentMenu = BL.Menu.GetParentMenus(DI.dBAccess);
            return View();
        }

        [HttpPost]
        public IActionResult MenuCreate([Bind] MenuMasterModel menu)
        {
            ActiveUser av = FormsAuthentication.GetCurrentUser(DI.session);
            if (ModelState.IsValid)
            {
                try
                {
                    DataSet dataSet = BL.Menu.InsertMenuMaster(menu.MenuName, menu.MenuDesc, menu.Code, menu.MenuUrl, menu.MenuIcon
                         , menu.MenuSrno, av.UserCode.ToString(), menu.CONTROLLERNAME, menu.ACTIONNAME, menu.AREANAME, DI.dBAccess);
                    if (dataSet.Tables[0].Rows.Count > 0)
                    {
                        if (dataSet.Tables[0].Rows[0][0].ToString() == "Saved")
                        {
                            TempData["Message"] = "success|Menu added successfully";
                            return RedirectToAction("Index");
                        }
                        else if (dataSet.Tables[0].Rows[0][0].ToString() == "Error")
                        {
                            TempData["Message"] = "error|Same menu already exists";
                            return RedirectToAction("MenuCreate");
                        }
                    }
                    else
                    {
                        return RedirectToAction("MenuCreate");
                    }
                    return RedirectToAction("MenuCreate");
                }
                catch (Exception ex)
                {
                    FormsAuthentication.LogException(ex, Request, DI.session, "MenuMaster", "Create", DI.dBAccess);
                    TempData["Message"] = "error|Error occurred while creating menu!";
                    return RedirectToAction("MenuCreate");
                }

            }
            else
            {
                TempData["Message"] = "error|Data cannot be empty";
                return RedirectToAction("MenuCreate");
            }


        }
        public IActionResult Edit(string Code)
        {
            try
            {

                DataSet dataSet = BL.Menu.SelectMenuMaster(Code, DI.dBAccess);
                List<MenuMasterModel> menuMaster = new List<MenuMasterModel>();
                for (int i = 0; i < dataSet.Tables[0].Rows.Count; i++)
                {
                    menuMaster.Add(new MenuMasterModel
                    {
                        MenuName = dataSet.Tables[0].Rows[i]["MenuName"].ToString(),
                        MenuDesc = dataSet.Tables[0].Rows[i]["MenuDesc"].ToString(),
                        MenuUrl = dataSet.Tables[0].Rows[i]["MenuUrl"].ToString(),
                        MenuIcon = dataSet.Tables[0].Rows[i]["MenuIcon"].ToString(),
                        MenuSrno = dataSet.Tables[0].Rows[i]["MenuSrno"].ToString(),
                        ParentId = dataSet.Tables[0].Rows[i]["ParentId"].ToString(),
                        CODE = dataSet.Tables[0].Rows[i]["Code"].ToString(),
                        LOCKED = Convert.ToBoolean(dataSet.Tables[0].Rows[i]["Locked"].ToString()),
                        CONTROLLERNAME = dataSet.Tables[0].Rows[i]["CONTROLLERNAME"].ToString(),
                        ACTIONNAME = dataSet.Tables[0].Rows[i]["ACTIONNAME"].ToString(),
                        AREANAME = dataSet.Tables[0].Rows[i]["AREANAME"].ToString()
                    });
                }
                ViewBag.ParentMenu = BL.Menu.GetParentMenus(DI.dBAccess);
                return View("Edit", menuMaster);
            }
            catch (Exception ex)
            {
                FormsAuthentication.LogException(ex, Request, DI.session, "MenuMaster", "Edit(string code)", DI.dBAccess);
                return View();
            }
        }
        [HttpPost]


        public IActionResult Edit([Bind] MenuMasterModel menu)
        {

            try
            {
                ActiveUser av = FormsAuthentication.GetCurrentUser(DI.session);
                DataSet dataSet = BL.Menu.UpdateMenuMaster(menu.Code, menu.MenuName, menu.MenuDesc, menu.ParentId, menu.MenuUrl,
                                                          menu.CONTROLLERNAME, menu.ACTIONNAME, menu.MenuIcon, menu.MenuSrno,
                                                          Convert.ToBoolean(menu.Locked),Convert.ToInt32( av.UserCode), menu.AREANAME, DI.dBAccess);
                TempData["Message"] = "success|Menu updated successfully";
            }
            catch (Exception ex)
            {
                FormsAuthentication.LogException(ex, Request, DI.session, "MenuMaster", "Edit", DI.dBAccess);
                TempData["Message"] = "error|Error occurred while updating record";
            }
            return RedirectToAction("Index");
        }

        public IActionResult Delete(int Code)
        {
            try
            {
                ActiveUser av = FormsAuthentication.GetCurrentUser(DI.session);
                DataSet dataSet = BL.Menu.MenuDelete(Code,Convert.ToInt32(av.UserCode), DI.dBAccess);
                TempData["Message"] = "success|Menu deleted successfully";
            }
            catch (Exception ex)
            {
                FormsAuthentication.LogException(ex, Request, DI.session, "MenuMaster", "Delete", DI.dBAccess);
                TempData["Message"] = "error|Error occurred while deleting record";
            }
            return RedirectToAction("Index");
        }
    }
}
