using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MasterApplication.Services
{
    public class ActiveUser
    {
        public int UserCode { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string LastLoginDateTime { get; set; }
        public string ExpiryDateTime { get; set; }
        public bool IsADuser { get; set; }
        public bool IsLocked { get; set; }
        public int LoginAttemptsCount { get; set; }
        public string Designation { get; set; }
        public string DateOfJoining { get; set; }
        public string ImageName { get; set; }
        public string ProductName { get; set; }
        public string BankName { get; set; }
        public string LoginTime { get; set; }
        public string loginSession { get; set; }
        public bool IsAdmin { get; set; }

        public bool isEdit { get; set; }

        public bool isDelete { get; set; }
    }
}
