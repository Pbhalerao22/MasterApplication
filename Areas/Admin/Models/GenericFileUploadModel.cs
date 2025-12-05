using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MasterApplication.Areas.Admin.Models
{
    public class GenericFileUploadModel
    {
        public IFormFile FU_GenericFile { get; set; }
        public  string TXT_FilePath { get; set; }
        public string RupayXMLType { get; set; }
    }
    public class RupayXMLType
    {
        public string XMLType { get; set; }
        public string Value { get; set; }
    }
}
