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
    public class RoleMenuMappingController : Controller
    {
        readonly DependancyInjection DI;

        public RoleMenuMappingController(DependancyInjection _DI)
        {
            DI = _DI;
        }
        public IActionResult Index()
        {
            ViewBag.SelectRole = BL.Role.GetSelectRoles(DI.dBAccess);
            return View();
        }
        [HttpPost]
        public IActionResult GetRoles()
        {
            string JsonString = Request.Form.Keys.FirstOrDefault();
            JObject JArray = JObject.Parse(JsonString);

            int start = Convert.ToInt16(JArray["PageNo"].ToString());
            int length = Convert.ToInt16(JArray["PageSize"].ToString());
            string strSearchColumn = JArray["SearchColumn"].ToString();
            string strSearchValue = JArray["SearchValue"].ToString();
            string strSortColumn = JArray["SortColumn"].ToString();
            string strSortType = JArray["SortType"].ToString();
            string strRoleCode = JArray["RoleCode"].ToString();

            int pageSize = length != null ? Convert.ToInt32(length) : 0;
            int skip = start != null ? Convert.ToInt32(start * pageSize) : 0;
            int recordsTotal = 0;
            DataSet dataSet1 = BL.Role.GetRoleMenuMapping(strRoleCode, DI.dBAccess);

            DataSet dataSet = DI.commonClass.GetMasterForGridRoleMap_ADM(strSearchValue, strSearchColumn, strSortColumn,
                                                               strSortType, start, length, "VW_NCORE_ROLEMENUMAPPING", strRoleCode, DI);

            int recordsFiltered = Convert.ToInt32(dataSet.Tables[0].Rows[0]["TotalRecords"]);
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

        public IActionResult RoleMenuMapping()
        {
            ViewBag.SelectRole = BL.Role.GetSelectRoles(DI.dBAccess);
            return View();
        }


        [HttpPost]
        public IActionResult Test(RoleMenuSummaryClass list)
        {
            ViewBag.SelectRole = BL.Role.GetSelectRoles(DI.dBAccess);
            return View("Index");
        }


        [HttpPost]
        //public async Task<IActionResult> UpdateRoleMenu([FromBody] RoleMenuClassArray role)
        public ActionResult UpdateRoleMenu([FromBody] List<RoleMenuClassArray> role)
        {
            ActiveUser av = FormsAuthentication.GetCurrentUser(DI.session);
            try
            {
                for (int i = 0; i < role.Count; i++)
                {
                    BL.Role.InsertRoleMaster(role[i].RoleCode, role[i].MenuName, Convert.ToBoolean(role[i].IsAssgined), av.UserCode.ToString(), DI.dBAccess);
                }

                var jsonData = new
                {
                    Status = "saved"
                };
                return Ok(jsonData);
            }
            catch (Exception ex)
            {
                FormsAuthentication.LogException(ex, Request, DI.session, "RoleMenuMapping", "Create", DI.dBAccess);
                var jsonData = new
                {
                    Status = "error"
                };
                return Ok(jsonData);

            }
        }
    }
}
