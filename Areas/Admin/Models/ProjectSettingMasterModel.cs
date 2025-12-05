using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MasterApplication.Areas.Admin.Models
{
    public class ProjectSettingMasterModel
    {
        public string CODE { get; set; }
        public string PROJECT_NAME { get; set; }
        public string KEYNAME { get; set; }
        public string KEYVALUE { get; set; }
        public bool LOCKED { get; set; }
    }
}
