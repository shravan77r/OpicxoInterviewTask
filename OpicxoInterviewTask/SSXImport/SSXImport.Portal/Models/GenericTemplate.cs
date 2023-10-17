using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace SSXImport.Portal.Models
{
    
    public class GenericTemplate : BaseModel
    {
        public int? EmailTemplateId { get; set; }
      

        [Required(ErrorMessage = "Please Enter Template Name")]
        public string EmailTemplateName { get; set; }

        public string EmailTemplateDesc { get; set; }

        public string EmailTemplateFile { get; set; }

        public string EmailTemplateGUID { get; set; }


        public string EmailTemplatHiddenDesc { get; set; }

        [Required(ErrorMessage = "Please Select Object Type")]
        public string ObjectType { get; set; }

        [Required(ErrorMessage = "Please Select Object")]
        public string Object { get; set; }

        public List<MasterItem> ObjectTypeList { get; set; }

        public List<MasterItem> ObjectList { get; set; }

        public string[] FieldArray { get; set; }

        public List<EmailTemplateContent> ListEmailTemplateContent { get; set; }

        public DataTable ObjectDataTable { get; set; }
    }


    public class EmailTemplateList
    {
        public string RowNo { get; set; }
        public string EmailTemplateId { get; set; }
        public string EmailTemplateName { get; set; }
        public string EmailTemplateDesc { get; set; }
        public string EmailTemplateFile { get; set; }
        public string EmailTemplatHiddenDescTarget { get; set; }

        public string EmailTemplateGUID { get; set; }

        public string ObjectType { get; set; }
        public string Object { get; set; }

       
    }


    public class EmailTemplateContent
    {
        public string Content { get; set; }
    }
 }
