using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MasterApplication.Areas.Admin.Models
{
    public class RoleMenuMappingModel
    {
        public string RoleCode { get; set; }
        public string RoleName { get; set; }
        public string MenuName { get; set; }
        public string IsAssigned { get; set; }
        public Object ViewBag { get; set; }

    }
    public class SelectRole
    {
        public string RoleName { get; set; }
        public int Code { get; set; }
    }

    public class rolemaster
    {
        public MenuMasterModel MenuMaster { get; set; }
    }

    public class RoleMenuSummaryClass
    {
        public string RoleCode { get; set; }

        public string MenuName { get; set; }

        public string IsAssgined { get; set; }
    }
    public class RoleMenuClass
    {
        public RoleMenuClassArray[] Array { get; set; }
    }

    public class RoleMenuClassArray
    {
        public string RoleCode { get; set; }

        public string MenuName { get; set; }

        public string IsAssgined { get; set; }

    }
}
