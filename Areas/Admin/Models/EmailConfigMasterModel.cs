using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MasterApplication.Areas.Admin.Models
{
    public class EmailConfigMasterModel
    {
        public string CODE { get; set; }
        public string PROCESS_TYPE { get; set; }
        public string PROCESS_SUB_TYPE { get; set; }
        public string EMAIL_CONTENT { get; set; }
        public string ATTACHMENT_PATH { get; set; }
        public string ATTACHMENT_NAME { get; set; }

        public IFormFile EmailContentFile { get; set; }
        public List<IFormFile> AttachmentFile { get; set; }

    }
    /*public class EMAIL_CONTENT
    {
        public string FILE_UPLOAD { get; set; }
        public int Value { get; set; }
    }*/
}
