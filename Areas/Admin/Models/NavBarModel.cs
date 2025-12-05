using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MasterApplication.Areas.Admin.Models
{
    public class NavBarModel
    {
    }
    public class MainMenuModel
    {
        public List<MenuModel> menuModel { get; set; }
        public string LoginURL { get; set; }
    }
    public class MenuModel
    {
        public int MENUCODE { get; set; }
        public string MENUNAME { get; set; }
        public string MENUDESC { get; set; }
        public int PARENTID { get; set; }
        public string MENUURL { get; set; }
        public string MENUICON { get; set; }
        public int ISTOPMENU { get; set; }
        public int MENUSRNO { get; set; }

        public string CONTROLLERNAME { get; set; }
        public string ActionName { get; set; }
        public string AreaName { get; set; }

    }

    public class MenuList
    {
        public List<MenuModel> Items { get; set; }
    }

    public class CommanModal_1
    {
        public List<MenuModel> List_Menus { get; set; }
        public List<UserAssignedRoles> List_Roles { get; set; }
    }

    public class UserAssignedRoles
    {
        public int RoleCode { get; set; }
        public string RoleName { get; set; }
        public bool IsAssigned { get; set; }
        public int UserCode { get; set; }

        public bool DefaultRole { get; set; }
    }
}
