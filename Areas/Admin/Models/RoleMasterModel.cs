using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MasterApplication.Areas.Admin.Models
{
    public class RoleMasterModel
    {
        public string CODE { get; set; }
        public string ROLENAME { get; set; }
        public string ROLEDESCRIPTION { get; set; }
        public bool LOCKED { get; set; }
    }
}
