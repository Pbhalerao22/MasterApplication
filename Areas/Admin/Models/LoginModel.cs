using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MasterApplication.Areas.Admin.Models
{
    public class LoginModel
    {
        [Display(Name = "UserName")]
        [Required(ErrorMessage = "UserName cannot be empty!")]
        public string TXT_UserName { get; set; }


        [Required(ErrorMessage = "Password cannot be empty!")]
        [Display(Name = "Password")]
        [DataType(DataType.Password)]
        public string TXT_Password { get; set; }
        public string TXT_ENC_Password { get; set; }
    }
}
