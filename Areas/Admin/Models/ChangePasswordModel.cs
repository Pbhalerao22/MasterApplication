using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MasterApplication.Areas.Admin.Models
{
    public class ChangePasswordModel
    {

        public string username { get; set; }

        public string password { get; set; }
        public string re_password { get; set; }
    }
}
