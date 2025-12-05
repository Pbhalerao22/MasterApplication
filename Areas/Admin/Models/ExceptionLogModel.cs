using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MasterApplication.Areas.Admin.Models
{
    public class ExceptionLogModel
    {
        public string Code { get; set; }
        public string MachineName { get; set; }
        public string Url { get; set; }
        public string FormName { get; set; }
        public string MethodName { get; set; }
        public string ExceptionDetails { get; set; }
        public string LOGDATE { get; set; }
    }
}
