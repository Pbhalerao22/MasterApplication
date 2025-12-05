using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MasterApplication.Areas.Admin.Models
{
    public class MenuMasterModel
    {
        public string Code { get; set; }
        public string MenuName { get; set; }
        public string MenuDesc { get; set; }
        public string MenuUrl { get; set; }
        public string MenuIcon { get; set; }
        public string MenuSrno { get; set; }
        public bool LOCKED { get; set; }
        public string CODE { get; set; }
        //public int ParentID { get; set; }
        public string ParentId { get; set; }
        public string LastModifiedBy { get; set; }
        public string Locked { get; set; }
        public string CONTROLLERNAME { get; set; }
        public string ACTIONNAME { get; set; }
        public string AREANAME { get; set; }
    }
    public class ParentMenus
    {
        public string MenuName { get; set; }
        public int ParentId { get; set; }
        public int Code { get; set; }

    }
    public class menuMaster
    {
        public MenuMasterModel MenuMasterModel { get; set; }
    }
}
