using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MasterApplication.Services
{
    public class MyAppSettings
    {
        public string OracleConnection { get; set; }
        public string OracleConnection_ADM { get; set; }
        public string SQLConnection_ADM { get; set; }
        public int BatchSize { get; set; }
        public bool UseSqlConnection { get; set; }

        public string EmailTemplateCommonPath { get; set; }
        public string RupayValue { get; set; }

        public string LoginURL { get; set; }
      
        public string LDAP_DOMAIN_NAME { get; set; }
       
    }
}
