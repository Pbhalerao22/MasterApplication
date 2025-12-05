using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MasterApplication.Areas.Admin.Models
{
    public class UserRoleModel
    {
        public int UserCode { get; set; }
        public List<UserList> userLists { get; set; }
        public List<UserRoleList> userRoleLists { get; set; }
    }

    public class UserList
    {
        public int UserCode { get; set; }
        public string UserName { get; set; }
    }
    public class UserRoleList
    {
        public int RoleCode { get; set; }
        public string RoleName { get; set; }
        public bool IsAssigned { get; set; }
        public int UserCode { get; set; }
        public bool DefaultRole { get; set; }
    }
}
