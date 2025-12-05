using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MasterApplication.Areas.Admin.Models
{
    public class SecurityQuestionsModel
    {
        public string CODE { get; set; }
        public string Question { get; set; }
        public bool LOCKED { get; set; }
    }
}
