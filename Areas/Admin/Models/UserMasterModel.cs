using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MasterApplication.Areas.Admin.Models
{
    public class UserMasterModel
    {
        public string CODE { get; set; }
        public string USERNAME { get; set; }
        public string PASSWORD { get; set; }
        public string FULLNAME { get; set; }
        public string SECURITYQUESTION { get; set; }
        public string SECURITYANSWER { get; set; }
        public bool ISADUSER { get; set; }
        public bool ISADMIN { get; set; }
        public bool LOCKED { get; set; }

        public string ISEDIT { get; set; }
        public string ISDELETE { get; set; }
        public int LOGINATTEMPTS { get; set; }
    }
    public class SecurityQuestion
    {
        public string SECURITYQUESTION { get; set; }
        public int Value { get; set; }
    }

    public class UserMaster
    {
        public UserMasterModel userMasterModel { get; set; }
        public List<SecurityQuestion> securityQuestion { get; set; }
    }
}
