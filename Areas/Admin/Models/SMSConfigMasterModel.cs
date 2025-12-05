using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MasterApplication.Areas.Admin.Models
{
    public class SMSConfigMasterModel
    {
        public string CODE { get; set; }
        public string PROCESS_TYPE { get; set; }
        public string PROCESS_SUB_TYPE { get; set; }
        public string SMS_CONTENT { get; set; }    
        public string OVERDUE_DATE { get; set; }
        public string NUMBER_OF_DAYS { get; set; }
    }
}
