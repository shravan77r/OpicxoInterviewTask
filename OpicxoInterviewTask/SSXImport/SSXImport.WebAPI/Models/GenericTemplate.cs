using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SSXImport.WebAPI.Models
{
    
    public class GenericTemplate : BaseModel
    {
        public int? EmailTemplateId { get; set; }
        public string EmailTemplateGUID { get; set; }
        public string EmailTemplateName { get; set; }

        public string EmailTemplateDesc { get; set; }

        public string EmailTemplateFile { get; set; }

        public string EmailTemplatHiddenDesc { get; set; }

        public string ObjectType { get; set; }

        public string Object { get; set; }

    }
}
